using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.RedisModule.CacheHelper;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.RedisModule
{
    public interface ICacheManager : ISingletonDependency
    {
        #region 设置缓存 
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        void SetCache(string key, object value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        Task SetCacheAsync(string key, object value);

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        void SetCache(string key, object value, TimeSpan timeout);

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        Task SetCacheAsync(string key, object value, TimeSpan timeout);

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="expireType">过期类型</param>  
        void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType);

        /// <summary>
        /// 设置缓存
        /// 注：默认过期类型为绝对过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="expireType">过期类型</param>  
        Task SetCacheAsync(string key, object value, TimeSpan timeout, ExpireType expireType);
        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        string GetCache(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task<string> GetCacheAsync(string key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        T GetCache<T>(string key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task<T> GetCacheAsync<T>(string key);

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);


        /// <summary>
        /// 查询没有写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataRetriever"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<T> GetCacheAsync<T>(string key, Func<Task<T>> dataRetriever, TimeSpan timeout);

        #endregion

        #region 删除缓存

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        void RemoveCache(string key);

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task RemoveCacheAsync(string key);

        Task RemoveByPrefixAsync(string key);

        #endregion

        #region 刷新缓存
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        void RefreshCache(string key);
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task RefreshCacheAsync(string key);
        #endregion
    }
}
