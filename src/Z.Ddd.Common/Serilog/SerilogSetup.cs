﻿using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Z.Ddd.Common.LogHelper;
using Z.Ddd.Common.Serilog.Extensions;

namespace Z.Ddd.Common.Serilog;

public static class SerilogSetup
{
    public static IHostBuilder AddSerilogSetup(this IHostBuilder host)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            //记录Net Core系统和EF日志最低级别
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .ReadFrom.Configuration(AppSettings.Configuration)
            .Enrich.FromLogContext()
            //输出到控制台
            .WriteToConsole()
            //将日志保存到文件中
            .WriteToFile();


        Log.Logger = loggerConfiguration.CreateLogger();

        //Serilog 内部日志
        var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
        SelfLog.Enable(TextWriter.Synchronized(file));

        host.UseSerilog();
        return host;
    }
}