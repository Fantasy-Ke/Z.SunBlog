using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.RedisModule.CacheHelper;

namespace Z.Fantasy.Core.RedisModule
{
    public static class RedisExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            //使用CsRedis
            var cacheOption = configuration.GetSection("App:Cache").Get<CacheOptions>()!;

            if (cacheOption == null)
            {
                throw new UserFriendlyException("无法获取App:Cache  redis缓存配置");
            }

            switch (cacheOption.CacheType)
            {
                case CacheType.Memory: services.AddDistributedMemoryCache(); break;
                case CacheType.Redis:
                    {
                        var csredis = new CSRedisClient(cacheOption.RedisEndpoint);
                        RedisHelper.Initialization(csredis);
                        services.AddSingleton<IDistributedCache>(new CSRedisCache(csredis));
                    }; break;
                default: throw new Exception("缓存类型无效");
            }
        }
    }
}
