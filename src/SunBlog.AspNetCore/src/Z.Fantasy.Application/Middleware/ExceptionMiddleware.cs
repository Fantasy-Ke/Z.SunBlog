using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Z.Fantasy.Core.Extensions;
using System.Runtime.ExceptionServices;
using UAParser;
using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.Entities.EntityLog;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.ResultResponse;
using Z.Fantasy.Core.UnitOfWork;
using Z.Fantasy.Core.UserSession;

namespace Z.Fantasy.Application.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private IHostEnvironment _environment;
    private readonly IUserSession _userSession;
    readonly ILogger<ExceptionMiddleware> logger;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionMiddleware> logger, IUserSession userSession)
    {
        _next = next;
        _environment = environment;
        this.logger = logger;
        _userSession = userSession;
    }

    public async Task Invoke(HttpContext context)
    {
        ExceptionDispatchInfo edi;
        try
        {
            var task = _next(context);
            //没有完成继续异步委托执行
            if (!task.IsCompletedSuccessfully)
            {
                await Awaited(context, () => task);
            }
            return;
        }
        catch (Exception e)
        {
            edi = ExceptionDispatchInfo.Capture(e);
        }

        await HandleException(context, edi);
    }

    private async Task HandleException(HttpContext context, ExceptionDispatchInfo edi)
    {
        if (context.Response.HasStarted)
        {
            // 响应开始抛出异常终止响应
            edi.Throw();
        }
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var requestPath = context.Request.Path;
        var exception = edi.SourceException;
        var logMsg = $"SourseRoute {requestPath} {exception.Source},错误信息：{exception.Message} {exception.StackTrace} ";
        Log.Error(logMsg);
        ZEngineResponse response = new ZEngineResponse(new ErrorInfo()
        {
            Message = _environment.IsDevelopment() ? logMsg : exception.Message
        }, false);
        response.Success = false;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(response);

        context.Response.OnCompleted(async () =>
        {
            var request = context.Request;
            string requestUri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            string ipAddress = App.GetRemoteIp(context);
            string requestAgent = context.Request.Headers.UserAgent.NotNullString();
            using var unit = IOCManager.GetService<IUnitOfWork>();
            try
            {
                var service = IOCManager.GetService<IBasicRepository<ZExceptionLog>>();
                await unit.BeginTransactionAsync();
                var uaParser = Parser.GetDefault();
                ClientInfo info = uaParser.Parse(requestAgent);
                var entity = await service.InsertAsync(new ZExceptionLog()
                {
                    RequestUri = requestUri,
                    Message = exception.Message,
                    Source = exception.Source,
                    StackTrace = exception.StackTrace,
                    Type = _userSession.UserId,
                    OperatorId = _userSession.UserId,
                    ClientIP = ipAddress,
                    OperatorName = _userSession.UserName,
                    UserOS = $"{info.OS}",
                    UserAgent = $"{info.UA}",
                });
                await unit.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await unit.RollbackTransactionAsync();
                logger.LogError(ex, $"插入异常日志失败={ex.Message}（requestUri={requestUri}，requestAgent={requestAgent}，ipAddress={ipAddress}）");
            }
        });
    }


    //捕获后续管道的异常
    private async Task Awaited(HttpContext context, Func<Task> func)
    {
        ExceptionDispatchInfo? edi = null;
        try
        {
            await func.Invoke();
        }
        catch (Exception exception)
        {
            edi = ExceptionDispatchInfo.Capture(exception);
        }
        if (edi != null)
        {
            await HandleException(context, edi);
        }
    }
}
