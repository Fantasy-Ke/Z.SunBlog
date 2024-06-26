﻿using Microsoft.Extensions.DependencyInjection;
using Z.EventBus.Extensions;
using Z.Fantasy.Core;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;
using Z.HangFire.Builder;
using Z.OSSCore;
using Z.FreeRedis;

namespace Z.SunBlog.Core
{
    [DependOn(typeof(ZFantasyCoreModule))]
    public class SunBlogCoreModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            var configuration = context.GetConfiguration();
            context.Services.RegisterJobs();
            //redis注册
            context.Services.AddZRedis(configuration, option =>
            {
                option.Capacity = 6;
            });

            // context.Services.AddZMinio(configuration);
            context.Services.AddOSSService(option =>
            {
                option.Provider = OSSProvider.Minio;
            });
            
            // 注入事件总线
            context.Services.AddEventBus();

            context.Services.AddSignalR()
                .AddMessagePackProtocol();
            //.AddStackExchangeRedis(o =>
            //{
            //    o.ConnectionFactory = async writer =>
            //    {
            //        //使用CsRedis
            //        var cacheOption = configuration.GetSection("App:Cache").Get<CacheOptions>()!;
            //        var connection = await ConnectionMultiplexer.ConnectAsync(cacheOption.RedisEndpoint, writer);
            //        connection.ConnectionFailed += (_, e) =>
            //        {
            //            Console.WriteLine("Connection to Redis failed.");
            //        };

            //        if (!connection.IsConnected)
            //        {
            //            Console.WriteLine("Did not connect to Redis.");
            //        }

            //        return connection;
            //    };
            //});
            
        }

        public override async Task PostInitApplicationAsync(InitApplicationContext context)
        {
            await context.ServiceProvider.InitChannlesAsync();
        }
    }
}