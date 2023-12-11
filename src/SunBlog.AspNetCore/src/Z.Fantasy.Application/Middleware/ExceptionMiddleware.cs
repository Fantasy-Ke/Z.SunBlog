using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Runtime.ExceptionServices;
using Z.Fantasy.Core.ResultResponse;

namespace Z.Fantasy.Application.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private IHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
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
