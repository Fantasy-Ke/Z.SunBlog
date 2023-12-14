using FreeRedis;
using StackExchange.Redis;

namespace Z.Fantasy.Core.RedisModule
{
    public partial interface IRedisCacheBaseService
    {
        #region 普通

        /// <summary>
        /// 如果key已经存在并且是一个字符串，APPEND 命令将指定的value追加到该key原来值（value）的末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        Task<long> AppendAsync(string key,string value);

        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<string> GetAsync(string key);
        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<string> GetSetAsync(string key, string value);
        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<string> GetSetAsync<T>(string key, T value);

        /// <summary>
        /// 用于在key存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> DelAsync(params string[] keys);

        /// <summary>
        /// 检查给定key是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, int seconds);
        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> ExpireAtAsync(string key, DateTime expiry);

        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        Task<long> IncrByAsync(string key, long value = 1);
        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        Task<decimal> IncrByFloatAsync(string key, decimal value);

        /// <summary>
        /// 修改key的名称
        /// </summary>
        /// <param name="key"> 旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        Task RenameAsync(string key, string newKey);

        /// <summary>
        /// 设置指定key的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        Task SetAsync(string key, string value, int expireSeconds = -1);

        #endregion

        #region 哈希表（Hash）

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        Task<long> HDelAsync(string key, params string[] fields);

        /// <summary>
        /// 查看哈希表key中，指定的字段是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        Task<bool> HExistsAsync(string key, string field);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        Task<string> HGetAsync(string key, string field);
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        Task<T> HGetAsync<T>(string key, string field);

        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> HGetAllAsync(string key);
        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, T>> HGetAllAsync<T>(string key);

        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        Task<long> HIncrByAsync(string key, string field, long value = 1);
        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        Task<decimal> HIncrByFloatAsync(string key, string field, decimal value);

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<string[]> HKeysAsync(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<long> HLenAsync(string key);

        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        Task<string[]> HMGetAsync(string key, params string[] fields);
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        Task<T[]> HMGetAsync<T>(string key, params string[] fields);

        /// <summary>
        /// 将哈希表key中的字段field的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> HSetAsync(string key, string field, string value);

        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> HSetNxAsync(string key, string field, string value);

        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<string[]> HValsAsync(string key);
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        Task<T[]> HValsAsync<T>(string key);

        #endregion

        #region 列表（List）



        #endregion

        #region 集合（Set）



        #endregion

        #region 有序集合（Sorted Set）

        /// <summary>
        /// 获取有序集合的长度
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="min">最小值（负无穷）</param>
        /// <param name="max">最小值（正无穷）</param>
        /// <returns></returns>
        Task<long> ZCountAsync(string key, decimal min = decimal.MinValue, decimal max = decimal.MaxValue);

        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="member">元素</param>
        /// <param name="score">分数</param>
        /// <returns></returns>
        Task<long> ZAddAsync(string key, decimal score, string member, params object[] scoreMembers);

        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">集合</param>
        /// <returns></returns>
        Task<long> ZAddAsync(string key, ZMember[] scoreMembers);

        /// <summary>
        /// 移除有序集合指定元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        Task<long> ZRemAsync(string key, params string[] members);

        #endregion
    }
}
