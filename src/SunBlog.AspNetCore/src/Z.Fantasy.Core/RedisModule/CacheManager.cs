using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.RedisModule.CacheHelper;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.RedisModule
{
    public class CacheManager : ICacheManager, ISingletonDependency
    {
        private readonly IDistributedCache _cache;
        private readonly bool isredis;
        public CacheManager(IDistributedCache cache)
        {
            _cache = cache;
            isredis = AppSettings.GetValue("App:Cache:CacheType")! == "Redis";
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
            _cache.SetString(cacheKey, value.ToJson());
        }

        public async Task SetCacheAsync(string key, object value)
        {
            string cacheKey = BuildKey(key);
            await _cache.SetStringAsync(cacheKey, value.ToJson());
        }

        public void SetCache(string key, object value, TimeSpan timeout)
        {
            string cacheKey = BuildKey(key);
            _cache.SetString(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
            });
        }

        public async Task SetCacheAsync(string key, object value, TimeSpan timeout)
        {
            string cacheKey = BuildKey(key);
            await _cache.SetStringAsync(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
            });
        }

        public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType)
        {
            string cacheKey = BuildKey(key);
            if (expireType == ExpireType.Absolute)
            {
                //这里没转换标准时间，Linux时区会有问题？
                _cache.SetString(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
                });
            }
            else
            {
                _cache.SetString(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = timeout
                });
            }
        }

        public async Task SetCacheAsync(string key, object value, TimeSpan timeout, ExpireType expireType)
        {
            string cacheKey = BuildKey(key);
            if (expireType == ExpireType.Absolute)
            {
                //这里没转换标准时间，Linux时区会有问题？
                await _cache.SetStringAsync(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
                });
            }
            else
            {
                await _cache.SetStringAsync(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = timeout
                });
            }
        }

        public string GetCache(string idKey)
        {
            if (idKey.IsNullOrEmpty())
            {
                return null;
            }
            string cacheKey = BuildKey(idKey);
            var cache = _cache.GetString(cacheKey);
            return cache;
        }
        public async Task<string> GetCacheAsync(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return null;
            }
            string cacheKey = BuildKey(key);
            var cache = await _cache.GetStringAsync(cacheKey);
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
            if (isredis)
            {
                using (var redisLock = RedisHelper.Lock(cacheKey, 10))
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
                    await _cache.SetStringAsync(cacheKey, item.ToJson(), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
                    });

                    redisLock.Dispose();

                    result = item;
                }
            }
            else
            {
                Task<T> task = dataRetriever();
                T item = await task;
                if (item == null) { return result; }
                await _cache.SetStringAsync(cacheKey, item.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now + timeout)
                });
            }
            return result;
        }

        public void RemoveCache(string key)
        {
            _cache.Remove(BuildKey(key));
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(BuildKey(key));
        }
        public async Task RemoveByPrefixAsync(string key)
        {
            if (isredis)
            {
                var keys = await RedisHelper.KeysAsync("*" + BuildKey(key) + "*");
                foreach (var item in keys)
                {
                    await _cache.RemoveAsync(item);
                }
            }

        }
        public void RefreshCache(string key)
        {
            _cache.Refresh(BuildKey(key));
        }

        public async Task RefreshCacheAsync(string key)
        {
            await _cache.RefreshAsync(BuildKey(key));
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return string.IsNullOrWhiteSpace(await GetCacheAsync(key)) ? false : true;
        }


    }
}
