using FreeRedis;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using NewLife.Caching;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Text;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.RedisModule.CacheHelper;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.RedisModule
{
    public class CacheManager : RedisCacheBaseService, ICacheManager, ISingletonDependency
    {
        private readonly RedisClient _redisClient;
        private readonly RedisCacheOptions cacheOptions;

        public CacheManager(RedisClient redisClient):base(redisClient)
        {
            _redisClient = redisClient;
            cacheOptions = AppSettings.AppOption<RedisCacheOptions>("App:RedisCache");
        }

        /// <summary>
        /// 创建缓存Key
        /// </summary>
        /// <param name="idKey"></param>
        /// <returns></returns>
        protected string BuildKey(string idKey)
        {
            return $"Cache_{GetType().FullName}_{idKey}";
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            string cacheKey = BuildKey(key);
            _redisClient.Set(cacheKey, value.ToJson());
        }

        public async Task SetCacheAsync(string key, object value)
        {
            string cacheKey = BuildKey(key);
            await _redisClient.SetAsync<string>(cacheKey, value.ToJson());
        }

        public void SetCache(string key, object value, TimeSpan timeout)
        {
            string cacheKey = BuildKey(key);
            _redisClient.Set<string>(cacheKey, value.ToJson(), timeout.Seconds);
        }

        public async Task SetCacheAsync(string key, object value, TimeSpan timeout)
        {
            string cacheKey = BuildKey(key);
            await _redisClient.SetAsync<string>(cacheKey, value.ToJson(), timeout.Seconds);
        }


        public string GetCache(string idKey)
        {
            if (idKey.IsNullOrEmpty())
            {
                return null;
            }
            string cacheKey = BuildKey(idKey);
            var cache = _redisClient.Get(cacheKey);
            return cache;
        }
        public async Task<string> GetCacheAsync(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return null;
            }
            string cacheKey = BuildKey(key);
            var cache = await _redisClient.GetAsync(cacheKey);
            return cache;
        }

        public T GetCache<T>(string key)
        {
            var cache = GetCache(key);
            if (!cache.IsNullOrEmpty())
            {
                return cache.ToObject<T>();
            }
            return default;
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cache = await GetCacheAsync(key);
            if (!string.IsNullOrEmpty(cache))
            {
                return cache.ToObject<T>();
            }
            return default;
        }

        public async Task<T> GetCacheAsync<T>(string key, Func<Task<T>> dataRetriever, TimeSpan timeout)
        {
            var result = await GetCacheAsync<T>(key);
            if (result != null)
            {
                return result;
            }
            string cacheKey = BuildKey(key);
            using (var redisLock = _redisClient.Lock(cacheKey, 10))
            {
                if (redisLock == null)
                {
                    throw new UserFriendlyException("抢不到所");
                }

                Task<T> task = dataRetriever();
                bool flag = !task.IsCompleted;
                if (flag)
                {
                    flag = await Task.WhenAny(task, Task.Delay(10)) != task;
                }
                //if (flag)
                //{
                //    throw new UserFriendlyException("任务执行错误");
                //}
                T item = await task;
                if (item == null) { return result; }
                await _redisClient.SetAsync(cacheKey, item.ToJson(), timeout.Seconds);

                redisLock.Dispose();

                result = item;
            }

            return result;
        }

        public void RemoveCache(string key)
        {
            _redisClient.Del(BuildKey(key));
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _redisClient.DelAsync(BuildKey(key));
        }
        public async Task RemoveByPrefixAsync(string key)
        {

            var keys = await _redisClient.KeysAsync("*" + BuildKey(key) + "*");
            foreach (var item in keys)
            {
                await _redisClient.DelAsync(item);
            }

        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _redisClient.ExistsAsync(BuildKey(key));
        }

    }
}
