using FreeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.RedisModule.CacheHelper;

namespace Z.Fantasy.Core.RedisModule
{
    public partial interface IRedisCacheBaseService
    {
        #region 普通类

        /// <summary>
        /// 如果key已经存在并且是一个字符串，APPEND 命令将指定的value追加到该key原来值（value）的末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        long Append(string key, object value);

        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        string GetSet(string key, object value);
        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        string GetSet<T>(string key, T value);

        /// <summary>
        /// 用于在key存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        long Del(params string[] keys);

        /// <summary>
        /// 检查给定key是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        bool Expire(string key, int seconds);
        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        bool ExpireAt(string key, DateTime expire);

        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        long IncrBy(string key, long value = 1);
        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        decimal IncrByFloat(string key, decimal value);

        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        string[] Keys(string pattern);

        /// <summary>
        /// 查看服务是否运行
        /// </summary>
        /// <returns></returns>
        string Ping();

        /// <summary>
        /// 修改key的名称
        /// </summary>
        /// <param name="key"> 旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        void Rename(string key, string newKey);

        /// <summary>
        /// 设置指定key的值，所有写入参数object都支持string | byte[]| 数值 | 对象
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        void Set(string key, object value, int expireSeconds = 0);

        /// <summary>
        /// 设置指定key的值，所有写入参数object都支持string | byte[]| 数值 | 对象
        /// </summary>
        /// <typeparam name="TData">消息对象类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="data">实体对象</param>
        /// <param name="expireSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        void Set<TData>(string key, TData data, int expireSeconds = 0);

        /// <summary>
        /// 返回所有给定键的值，对于其中个别键不存在，或其值不为字符串的，返回特殊的 nil 值，因此 MGET 指令永远不会执行失败
        /// </summary>
        /// <param name="keys">如：runoob*</param>
        /// <returns></returns>
        string[] MGet(params string[] keys);
        /// <summary>
        /// 将给定键的值设置为对应的新值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">键值</param>
        /// <param name="keyValues">其它键值对</param>
        /// <returns></returns>
        void MSet(string key, object value, params object[] keyValues);

        /// <summary>
        /// 返回键的剩余生存时间
        /// </summary>
        /// <param name="key"> 旧名称，不含prefix前辍</param>
        long Ttl(string key);

        #endregion

        #region 哈希表

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        long HDel(string key, params string[] fields);

        /// <summary>
        /// 查看哈希表key中，指定的字段是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        bool HExists(string key, string field);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        string HGet(string key, string field);
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        T HGet<T>(string key, string field);

        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> HGetAll(string key);
        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, T> HGetAll<T>(string key);

        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量v
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        long HIncrBy(string key, string field, long value = 1);
        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        decimal HIncrByFloat(string key, string field, decimal value);

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        string[] HKeys(string key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        long HLen(string key);

        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        string[] HMGet(string key, params string[] fields);
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        T[] HMGet<T>(string key, params string[] fields);

        /// <summary>
        /// 将哈希表key中的字段field的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        long HSet(string key, string field, object value);
        /// <summary>
        /// 将哈希表key中的字段field的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        long HSet<T>(string key, Dictionary<string, T> keyValues);
        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        void HMSet<T>(string key, string field, T value, params object[] keyValues);
        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool HSetNx(string key, string field, object value);

        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        string[] HVals(string key);
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        T[] HVals<T>(string key);

        #endregion

        #region 列表（List）

        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        string BLPop(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        T BLPop<T>(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns><param name="timeoutSeconds">超时时间（秒）</param>
        KeyValue<string> BLPop(string[] keys, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        KeyValue<T> BLPop<T>(string[] keys, int timeoutSeconds);

        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        string BRPop(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        T BRPop<T>(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        KeyValue<string> BRPop(string[] keys, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        KeyValue<T> BRPop<T>(string[] keys, int timeoutSeconds);

        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        string BRPopLPush(string source, string destination, int timeoutSeconds);
        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        T BRPopLPush<T>(string source, string destination, int timeoutSeconds);

        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        string LIndex(string key, long index);
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        T LIndex<T>(string key, long index);

        /// <summary>
        /// 指定列表中一个元素在它之前或之后插入另外一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="direction">插入方向（before|after）</param>
        /// <param name="pivot">参照元素</param>
        /// <param name="element">待插入的元素</param>
        /// <returns></returns>
        long LInsert(string key, InsertDirection direction, object pivot, object element);

        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long LLen(string key);

        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string LPop(string key);
        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T LPop<T>(string key);

        /// <summary>
        /// 获取列表 key 中匹配给定 element 成员的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="element">元素</param>
        /// <param name="rank">从第几个匹配开始计算</param>
        /// <returns></returns>
        long LPos<T>(string key, T element, int rank = 0);
        /// <summary>
        /// 获取列表 key 中匹配给定 element 成员的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="element">元素</param>
        /// <param name="rank">从第几个匹配开始计算</param>
        /// <param name="count">要匹配的总数</param>
        /// <param name="maxLen">只查找最多 len 个成员</param>
        /// <returns></returns>
        long[] LPos<T>(string key, T element, int rank, int count, int maxLen);

        /// <summary>
        /// 在列表头部插入一个或者多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        long LPush(string key, params object[] elements);

        /// <summary>
        /// 当储存列表的 key 存在时，用于将值插入到列表头部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        long LPushX(string key, params object[] elements);

        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        string[] LRange(string key, long start, long stop);
        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        T[] LRange<T>(string key, long start, long stop);

        /// <summary>
        /// 从列表中删除元素与 value 相等的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count">删除的数量（等于0时全部移除，小于0时从表尾开始向表头搜索，大于0时从表头开始向表尾搜索）</param>
        /// <param name="element">待删除的元素</param>
        /// <returns></returns>
        long LRem<T>(string key, long count, T element);

        /// <summary>
        /// 通过其索引设置列表中元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <param name="element">元素</param>
        void LSet<T>(string key, long index, T element);

        /// <summary>
        /// 保留列表中指定范围内的元素值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        void LTrim(string key, long start, long stop);

        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string RPop(string key);
        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T RPop<T>(string key);

        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        string RPopLPush(string source, string destination);
        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        T RPopLPush<T>(string source, string destination);

        /// <summary>
        /// 在列表中添加一个或多个值到列表尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        long RPush(string key, params object[] elements);

        /// <summary>
        /// 为已存在的列表添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        long RPushX(string key, params object[] elements);

        #endregion

        #region 集合（Set）

        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">集合成员</param>
        /// <returns>新增条数</returns>
        long SAdd(string key, params object[] members);

        /// <summary>
        /// 返回集合的大小（元素个数）
        /// </summary>
        /// <param name="key"></param>
        /// <returns>集合大小</returns>
        long SCard(string key);

        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SDiff(params string[] keys);
        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SDiff<T>(params string[] keys);

        /// <summary>
        /// 将多个集合的差集存储
        /// </summary>
        /// <param name="destination">存储差集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns>差集条数</returns>
        long SDiffStore(string destination, params string[] keys);

        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SInter(params string[] keys);

        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SInter<T>(params string[] keys);

        /// <summary>
        /// 将多个集合的交集存储
        /// </summary>
        /// <param name="destination">存储交集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns></returns>
        long SInterStore(string destination, params string[] keys);

        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SUnion(params string[] keys);

        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SUnion<T>(params string[] keys);

        /// <summary>
        /// 将多个集合的并集存储
        /// </summary>
        /// <param name="destination">存储并集的key</param>
        /// <param name="keys">多个集合的key</param>
        long SUnionStore(string destination, params string[] keys);

        /// <summary>
        /// 对象是否存在集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        bool SIsMember<T>(string key, T member);

        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] SMembers(string key);

        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T[] SMembers<T>(string key);

        /// <summary>
        /// 将集合内的元素移动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源集合key</param>
        /// <param name="destination">目标集合key</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        bool SMove<T>(string source, string destination, T member);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SPop(string key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T SPop<T>(string key);

        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        string[] SPop(string key, int count);

        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        T[] SPop<T>(string key, int count);

        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SRandMember(string key);

        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T SRandMember<T>(string key);

        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        string[] SRandMember(string key, int count);

        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        T[] SRandMember<T>(string key, int count);

        /// <summary>
        /// 移除集合中的多个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">元素</param>
        /// <returns></returns>
        long SRem(string key, params object[] members);
        /// <summary>
        /// 返回集合迭代器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ScanResult<string> SScan(string key, long cursor, string pattern, long count);

        #endregion

        #region 有序集合（Sorted Set）

        /// <summary>
        /// 获取有序集合的长度
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="min">最小值（负无穷）</param>
        /// <param name="max">最大值（正无穷）</param>
        /// <returns></returns>
        long ZCount(string key, decimal min = decimal.MinValue, decimal max = decimal.MaxValue);
        /// <summary>
        /// 获取有序集合的长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        long ZCount(string key, string min, string max);

        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素</param>
        /// <returns></returns>
        long ZAdd(string key, decimal score, string member, params object[] scoreMembers);
        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="scoreMembers">集合</param>
        /// <returns></returns>
        long ZAdd(string key, ZMember[] scoreMembers);

        /// <summary>
        /// 移除有序集合指定元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="members">元素数组</param>
        /// <returns></returns>
        long ZRem(string key, params string[] members);

        /// <summary>
        /// 获取有序集合指定元素的排名顺序
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        long? ZRank(string key, string member);

        /// <summary>
        /// 获取有序集合指定元素的指定区间内的成员
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns></returns>
        string[] ZRange(string key, decimal start = 0, decimal stop = -1);

        /// <summary>
        /// 获取元素个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ZCard(string key);

        /// <summary>
        /// 只有在member不存在时，向有序集合插入元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素数组</param>
        /// <returns></returns>
        long ZAddNx(string key, decimal score, string member, params object[] scoreMembers);
        /// <summary>
        /// 只有在member不存在时，向有序集合插入元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers">元素分数对象数组</param>
        /// <param name="than"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        long ZAddNx(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false);
        /// <summary>
        /// 只有在member存在时，更新有序集合元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素数组</param>
        /// <returns></returns>
        long ZAddXx(string key, decimal score, string member, params object[] scoreMembers);
        /// <summary>
        /// 只有在member存在时，更新有序集合元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers">元素数组</param>
        /// <param name="than"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        long ZAddXx(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false);

        /// <summary>
        /// 有序集合中对指定元素的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment">增量（默认为1）</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        decimal ZIncrBy(string key, decimal increment, string member);

        /// <summary>
        /// 计算 numkeys 个有序集合的交集，并且把结果放到 destination 中
        /// </summary>
        /// <param name="destination">目标key</param>
        /// <param name="keys"></param>
        /// <param name="weights">乘法因子（默认为1）</param>
        /// <param name="aggregate">结果集的聚合方式（默认为SUM）</param>
        /// <returns></returns>
        long ZInterStore(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null);

        /// <summary>
        /// 在有序集合中计算指定字典区间内元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        long ZLexCount(string key, string min, string max);

        /// <summary>
        /// 删除并返回最多count个有序集合key中最低得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ZMember ZPopMin(string key);
        /// <summary>
        /// 删除并返回最多count个有序集合key中最低得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        ZMember[] ZPopMin(string key, int count);

        /// <summary>
        /// 删除并返回最多count个有序集合key中的最高得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ZMember ZPopMax(string key);
        /// <summary>
        /// 删除并返回最多count个有序集合key中的最高得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        ZMember[] ZPopMax(string key, int count);

        /// <summary>
        /// 删除成员名称按字典由低到高排序介于min 和 max 之间的所有成员（集合中所有成员的分数相同）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByLex(string key, string min, string max);

        /// <summary>
        /// 移除有序集key中，指定排名(rank)区间 start 和 stop 内的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        long ZRemRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 移除有序集key中，所有score值介于min和max之间(包括等于min或max)的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByScore(string key, decimal min, decimal max);
        /// <summary>
        /// 移除有序集key中，所有score值介于min和max之间(包括等于min或max)的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByScore(string key, string min, string max);

        /// <summary>
        /// 获取有序集key中，指定区间内的成员（成员的位置按score值递减(从高到低)来排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        string[] ZRevRange(string key, decimal start, decimal stop);

        /// <summary>
        /// 获取有序集key中，指定区间内的成员+分数列表（成员的位置按score值递减(从高到低)来排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        ZMember[] ZRevRangeWithScores(string key, decimal start, decimal stop);

        /// <summary>
        /// 按字典从低到高排序，取索引范围内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] ZRevRangeByLex(string key, decimal max, decimal min, int offset = 0, int count = 0);
        /// <summary>
        /// 按字典从低到高排序，取索引范围内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] ZRevRangeByLex(string key, string max, string min, int offset = 0, int count = 0);

        /// <summary>
        /// 获取有序集合中指定分数区间的成员列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] ZRevRangeByScore(string key, decimal max, decimal min, int offset = 0, int count = 0);
        /// <summary>
        /// 获取有序集合中指定分数区间的成员列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] ZRevRangeByScore(string key, string max, string min, int offset = 0, int count = 0);

        /// <summary>
        /// 获取有序集合中指定分数区间的成员+分数列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ZMember[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, int offset = 0, int count = 0);
        /// <summary>
        /// 获取有序集合中指定分数区间的成员+分数列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ZMember[] ZRevRangeByScoreWithScores(string key, string max, string min, int offset = 0, int count = 0);

        /// <summary>
        /// 获取有序集key中成员member的排名（按score值从高到低排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long ZRevRank(string key, string member);



        /// <summary>
        /// 获取有序集key成员 member 的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        decimal? ZScore(string key, string member);

        /// <summary>
        /// 计算一个或多个有序集的并集，并存储在新的 key 中
        /// </summary>
        /// <param name="destination">目标key</param>
        /// <param name="keys"></param>
        /// <param name="weights">乘法因子（默认为1）</param>
        /// <param name="aggregate">结果集的聚合方式（默认为SUM）</param>
        /// <returns></returns>
        long ZUnionStore(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null);

        /// <summary>
        /// 随机返回N个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回的个数</param>
        /// <param name="repetition">是否允许有重复元素返回</param>
        /// <returns></returns>
        string[] ZRandMember(string key, int count, bool repetition);
        /// <summary>
        /// 随机返回N个元素, 包含分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回的个数</param>
        /// <param name="repetition">是否允许有重复元素返回</param>
        /// <returns></returns>
        ZMember[] ZRandMemberWithScores(string key, int count, bool repetition);

        #endregion

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
        #endregion
    }
}
