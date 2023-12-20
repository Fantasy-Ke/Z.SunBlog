using System;
using Hangfire;
using Hangfire.MySql;
using Hangfire.SqlServer;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.Helper;
using Z.Module.Reflection;
using Hangfire.Redis.StackExchange;
using Minio.DataModel;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Z.Fantasy.Core.HangFire.BackgroundJobs;

namespace Z.Fantasy.Core.HangFire.Builder;

public static class ZHangfireServiceBuilderExtensions
{
    public static void RegisterJobs(this IServiceCollection services)
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
    public static void ConfigureHangfireService(this IServiceCollection services, 
        Action<BackgroundJobServerOptions> optionsAction = null)
    {
        var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireEnabled");
        if (!enable) return;

        var options = new BackgroundJobServerOptions()
        {
            ShutdownTimeout = TimeSpan.FromMinutes(30),
            Queues = new string[] { "default", "jobs" }, //队列名称，只能为小写
            WorkerCount = 3, //Environment.ProcessorCount * 5, //并发任务数 Math.Max(Environment.ProcessorCount, 20)
            ServerName = "fantasy.hangfire",
        };
        optionsAction?.Invoke(options);
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)//向前兼容
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseZHangfireStorage()
            .UseSerilogLogProvider()
        ).AddHangfireServer(optionsAction: c => c = options);
    }


    /// <summary>
    /// 使用 Hangfire Storage
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IGlobalConfiguration UseZHangfireStorage(this IGlobalConfiguration configuration)
    {
        string connectionString = string.Empty;
        var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireRedis");
        if (enable)
        {
            var redisSrting = AppSettings.AppOption<string>("App:RedisCache:Configuration");
            var redisOptions = ConfigurationOptions.Parse(redisSrting);
            configuration.UseRedisStorage(ConnectionMultiplexer.Connect(redisOptions), new RedisStorageOptions
            {
                InvisibilityTimeout = TimeSpan.FromMinutes(30.0),
                FetchTimeout = TimeSpan.FromMinutes(3.0),
                ExpiryCheckInterval = TimeSpan.FromHours(1.0),
                Db = 2,
                Prefix = "Z_Fantasy:",
                SucceededListSize = 499,
                DeletedListSize = 499,
                LifoQueues = new string[0],
                UseTransactions = true,
            });
            goto redis;
        }
        switch (ZConfigBase.DatabaseType)
        {
            case DatabaseType.SqlServer:
                connectionString = AppSettings.AppOption<string>("App:ConnectionString:SqlServer");
                configuration.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    SchemaName = "Z_HangFire_"
                });
                break;
            case DatabaseType.MySql:
                connectionString = AppSettings.AppOption<string>("App:ConnectionString:Mysql");
                configuration.UseMysqlStorage(connectionString);
                break;
            default:
                throw new UserFriendlyException("不支持的数据库类型");
        }

    redis:
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
            TablesPrefix = "Z_HangFire_"
        });

        configuration.UseStorage(storage);

        return configuration;
    }
}