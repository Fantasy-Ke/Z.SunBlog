﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Web;
using UAParser;
using Z.EventBus.EventBus;
using Z.Fantasy.Application.Handlers;
using Z.Foundation.Core.Extensions;
using Z.Foundation.Core.Helper;
using Z.Foundation.Core.UserSession;

namespace Z.Fantasy.Application.Middleware
{
    public class RequestLogMiddleware
    {
        readonly RequestDelegate _next;
        readonly IUserSession _userSession;
        readonly ILogger<RequestLogMiddleware> logger;
        readonly Stopwatch stopwatch;
        private readonly ILocalEventBus _localEvent;
        public RequestLogMiddleware(RequestDelegate next, IUserSession userSession, ILogger<RequestLogMiddleware> logger, 
            ILocalEventBus localEvent)
        {
            _next = next;
            _userSession = userSession;
            this.logger = logger;
            stopwatch = new Stopwatch();
            _localEvent = localEvent;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestUri = context.Request.Path.NotNullString().TrimEnd('/');
            var requestMethod = context.Request.Method;
            if (AppSettings.GetValue("App:MiddlewareSettings:RequestLog:Enable").CastTo(false))
            {
                var ignoreApis = AppSettings.GetValue("App:MiddlewareSettings:RequestLog:IgnoreRequestApi");
                if (requestMethod?.ToLower() != "options" &&
                    requestUri.Contains("api", StringComparison.OrdinalIgnoreCase) &&
                    !ignoreApis.Contains(requestUri, StringComparison.OrdinalIgnoreCase))
                {
                    stopwatch.Restart();
                    HttpRequest request = context.Request;
                    DateTime requestTime = DateTime.Now;
                    string requestAgent = request.Headers["User-Agent"].NotNullString(), requestData = string.Empty, responseData = string.Empty,
                        ipAddress = App.GetRemoteIp(context);

                    if (requestMethod.ToLower() == "post" || requestMethod.ToLower() == "put")
                    {
                        request.EnableBuffering();
                        Stream stream = request.Body;
                        requestData = await new StreamReader(request.Body).ReadToEndAsync();
                        //重置请求指针读取
                        request.Body.Position = 0;
                    }
                    else if (requestMethod.ToLower() == "get" || requestMethod.ToLower() == "delete")
                    {
                        requestData = HttpUtility.UrlDecode(request.QueryString.NotNullString(), Encoding.UTF8);
                    }
                    if (requestMethod.ToLower() == "post" || requestMethod.ToLower() == "put" || requestMethod.ToLower() == "get" || requestMethod.ToLower() == "delete")
                    {
                        var originalBodyStream = context.Response.Body;
                        using var memoryStream = new MemoryStream();
                        context.Response.Body = memoryStream;
                        await _next(context);
                        responseData = await GetResponse(context.Response);
                        await memoryStream.CopyToAsync(originalBodyStream);
                        context.Response.Body = originalBodyStream;
                    }
                    else
                    {
                        await _next(context);
                    }

                    context.Response.OnCompleted(async () =>
                    {
                        stopwatch.Stop();
                        if (!AppSettings.GetValue("App:MiddlewareSettings:RequestLog:WriteDB").CastTo(false)) return;
                        var uaParser = Parser.GetDefault();
                        ClientInfo info = uaParser.Parse(requestAgent);
                        await _localEvent.EnqueueAsync(new RequestLogDto()
                        {
                            RequestUri = requestUri,
                            RequestType = requestMethod,
                            RequestData = requestData,
                            ResponseData = responseData,
                            UserId = _userSession.UserId,
                            UserName = _userSession.UserName,
                            ClientIP = ipAddress,
                            SpendTime = $"{stopwatch.ElapsedMilliseconds}ms",
                            UserOS = $"{info.OS}",
                            UserAgent = $"{info.UA}",
                        });
                        //using var unit = IOCManager.GetService<IUnitOfWork>();
                        //try
                        //{
                        //    unit.BeginTransaction();
                        //    var service = IOCManager.GetService<IBasicRepository<ZRequestLog>>();
                        //    var uaParser = Parser.GetDefault();
                        //    ClientInfo info = uaParser.Parse(requestAgent);
                        //    var entity = service.Insert(new ZRequestLog()
                        //    {
                        //        RequestUri = requestUri,
                        //        RequestType = requestMethod,
                        //        RequestData = requestData,
                        //        ResponseData = responseData,
                        //        UserId = _userSession.UserId,
                        //        UserName = _userSession.UserName,
                        //        ClientIP = ipAddress,
                        //        SpendTime = $"{stopwatch.ElapsedMilliseconds}ms",
                        //        UserOS = $"{info.OS}",
                        //        UserAgent = $"{info.UA}",
                        //    });
                        //    unit.CommitTransaction();
                        //}
                        //catch (Exception ex)
                        //{
                        //    unit.RollbackTransaction();
                        //    logger.LogError(ex, $"插入请求日志失败={ex.Message}（requestMethod={requestMethod}，requestUri={requestUri}，requestAgent={requestAgent}，ipAddress={ipAddress}）");
                        //}

                        //unit.Dispose();

                        //return Task.CompletedTask;
                    });
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                if ((requestMethod.ToLower() == "post" || requestMethod.ToLower() == "put") && requestUri != null)
                {
                    stopwatch.Restart();
                    context.Response.OnCompleted(() =>
                    {
                        stopwatch.Stop();
                        logger.LogInformation($"*请求监控：requestUri={requestUri}，耗时={stopwatch.ElapsedMilliseconds}ms");
                        return Task.CompletedTask;
                    });
                }
                await _next(context);
            }
        }

        /// <summary>
        /// 读取响应
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        static async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var resData = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return resData;
        }
    }
}
