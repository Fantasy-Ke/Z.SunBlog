﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Net;
using Z.Fantasy.Core.RedisModule;
using Z.EventBus.Extensions;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;
using Z.SunBlog.Common;
using Z.SunBlog.Core.Handlers.FileHandlers;
using Z.SunBlog.Core.Handlers.TestHandlers;
using Z.Fantasy.Core.Minio;
using Z.Fantasy.Core.HangFire.Builder;

namespace Z.SunBlog.Core
{
    [DependOn(typeof(SunBlogCommonModule))]
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

            context.Services.AddZMinio(configuration);
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