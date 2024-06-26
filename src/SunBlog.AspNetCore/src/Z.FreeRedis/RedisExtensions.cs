﻿using FreeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Z.Foundation.Core.Exceptions;

namespace Z.FreeRedis;

public static class RedisExtensions
{
    public static void AddZRedis(this IServiceCollection services, IConfiguration configuration, Action<ClientSideCachingOptions> clientChahingOptions = null)
    {
        //使用CsRedis
        var cacheOption = configuration.GetSection("App:RedisCache").Get<RedisCacheOptions>()!;

        if (cacheOption == null)
        {
            throw new UserFriendlyException("无法获取App:Cache  redis缓存配置");
        }

        if (!cacheOption.Enable)
            return;
        services.TryAddSingleton(x =>
        {
            var logger = x.GetRequiredService<ILogger<RedisClient>>();
            var redisClient = new RedisClient(cacheOption.Configuration);
            // 配置默认使用Newtonsoft.Json作为序列化工具
            redisClient.Serialize = JsonConvert.SerializeObject;
            redisClient.Deserialize = JsonConvert.DeserializeObject;
            redisClient.Notice += (s, e) => logger.LogInformation(e.Log);
            redisClient.Connected += (object sender, ConnectedEventArgs e) =>
            {
                logger.LogInformation($"RedisClient_Connected：{e.Host}");
            };
            redisClient.Unavailable += (object sender, UnavailableEventArgs e) =>
                   logger.LogInformation($"RedisClient_Connected：{e.Host}");
            if (cacheOption.SideCache.Enable)
            {
                var options = new ClientSideCachingOptions
                {
                    //本地缓存的容量
                    Capacity = cacheOption.SideCache.Capacity,
                    //过滤哪些键能被本地缓存
                    KeyFilter = key => key.StartsWith(cacheOption.SideCache.KeyFilterCache),
                    //检查长期未使用的缓存
                    CheckExpired = (key, dt) => DateTime.Now.Subtract(dt) > TimeSpan.FromMinutes(cacheOption.SideCache.ExpiredMinutes)
                };
                clientChahingOptions?.Invoke(options);
                redisClient.UseClientSideCaching(options);
            }
            return redisClient;
        });
    }
}
