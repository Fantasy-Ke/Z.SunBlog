using System;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.HangFire.BackgroundJobs;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.Helper;
using Z.Module.Reflection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Builder;

public static class ZHangfireServiceBuilderExtensions
{
    private static void RegisterJobs(IServiceCollection services)
    {
        var jobTypes = new List<Type>();

        services.OnRegistered(context =>
        {
            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IBackgroundJob<>)) ||
                ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IAsyncBackgroundJob<>)))
            {
                jobTypes.Add(context.ImplementationType);
            }
        });

        services.Configure<ZBackgroundJobOptions>(options =>
        {
            foreach (var jobType in jobTypes)
            {
                options.AddJob(jobType);
            }
        });
    }

    /// <summary>
    /// 注册Hangfire
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureHangfireService(this IServiceCollection services)
    {
        services.AddHangfire(config =>
        {
            object p = config.UseMedProHangfireStorage();
        });
        services.AddHangfireServer(optionsAction: c =>
        {
            //wait all jobs performed when BackgroundJobServer shutdown.
            c.ShutdownTimeout = TimeSpan.FromMinutes(30);
            c.Queues = new[] { "default", "jobs" }; //队列名称，只能为小写
            c.WorkerCount = 3; //Environment.ProcessorCount * 5, //并发任务数 Math.Max(Environment.ProcessorCount, 20)
            c.ServerName = "fantasy.hangfire";
        });
    }


    /// <summary>
    /// 使用 Hangfire Storage
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IGlobalConfiguration UseMedProHangfireStorage(this IGlobalConfiguration configuration)
    {
        var dbtype = AppSettings.AppOption<string>("App:DbType")!;
        string? connectionString = string.Empty;
        switch (dbtype.ToLower())
        {
            case "sqlserver":
                connectionString = AppSettings.AppOption<string>("App:ConnectionString:SqlServer");
                configuration.UseSqlServerStorage(connectionString);
                break;
            case "mysql":
                connectionString = AppSettings.AppOption<string>("App:ConnectionString:Mysql");
                configuration.UseMysqlStorage(connectionString);
                break;
            default:
                throw new UserFriendlyException("不支持的数据库类型");
        }

        return configuration;
    }

    /// <summary>
    /// 使用Oracle的Hangfire Storage
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IGlobalConfiguration UseMysqlStorage(this IGlobalConfiguration configuration, string connectionString)
    {

        var storage = new MySqlStorage(connectionString, new MySqlStorageOptions()
        {
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "fantasy"
        });

        configuration.UseStorage(storage);

        return configuration;
    }
}