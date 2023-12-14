using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.RedisModule
{
    public interface ICacheManager :IRedisCacheBaseService, ITransientDependency
    {
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
        Task<bool> ExistsCacheAsync(string key);


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

    }
}
