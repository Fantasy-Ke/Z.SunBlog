using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Z.Ddd.Common.Helper;

namespace Z.Ddd.Common.Serilog;

public static class SerilogSetup
{
    public static IHostBuilder AddSerilogSetup(this IHostBuilder host)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));

        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configuration = ConfigurationHelper.GetConfiguration("serilogs", environmentName: env);

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration);


        Log.Logger = loggerConfiguration.CreateLogger();

        //Serilog 内部日志
        var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
        SelfLog.Enable(TextWriter.Synchronized(file));

        host.UseSerilog();
        return host;
    }
}