using FreeRedis;

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
        Task<long> AppendAsync(string key, string value);

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

        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<string> BLPopAsync(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<T> BLPopAsync<T>(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns><param name="timeoutSeconds">超时时间（秒）</param>
        Task<KeyValue<string>> BLPopAsync(string[] keys, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<KeyValue<T>> BLPopAsync<T>(string[] keys, int timeoutSeconds);

        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<string> BRPopAsync(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<T> BRPopAsync<T>(string key, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<KeyValue<string>> BRPopAsync(string[] keys, int timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<KeyValue<T>> BRPopAsync<T>(string[] keys, int timeoutSeconds);

        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<string> BRPopLPushAsync(string source, string destination, int timeoutSeconds);
        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        Task<T> BRPopLPushAsync<T>(string source, string destination, int timeoutSeconds);

        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        Task<string> LIndexAsync(string key, long index);
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        Task<T> LIndexAsync<T>(string key, long index);

        /// <summary>
        /// 指定列表中一个元素在它之前或之后插入另外一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="direction">插入方向（before|after）</param>
        /// <param name="pivot">参照元素</param>
        /// <param name="element">待插入的元素</param>
        /// <returns></returns>
        Task<long> LInsertAsync(string key, InsertDirection direction, object pivot, object element);

        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> LLenAsync(string key);

        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> LPopAsync(string key);
        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> LPopAsync<T>(string key);

        /// <summary>
        /// 获取列表 key 中匹配给定 element 成员的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="element">元素</param>
        /// <param name="rank">从第几个匹配开始计算</param>
        /// <returns></returns>
        Task<long> LPosAsync<T>(string key, T element, int rank = 0);
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
        Task<long[]> LPosAsync<T>(string key, T element, int rank, int count, int maxLen);

        /// <summary>
        /// 在列表头部插入一个或者多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        Task<long> LPushAsync(string key, params object[] elements);

        /// <summary>
        /// 当储存列表的 key 存在时，用于将值插入到列表头部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        Task<long> LPushXAsync(string key, params object[] elements);

        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        Task<string[]> LRangeAsync(string key, long start, long stop);
        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        Task<T[]> LRangeAsync<T>(string key, long start, long stop);

        /// <summary>
        /// 从列表中删除元素与 value 相等的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count">删除的数量（等于0时全部移除，小于0时从表尾开始向表头搜索，大于0时从表头开始向表尾搜索）</param>
        /// <param name="element">待删除的元素</param>
        /// <returns></returns>
        Task<long> LRemAsync<T>(string key, long count, T element);

        /// <summary>
        /// 通过其索引设置列表中元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <param name="element">元素</param>
        Task LSetAsync<T>(string key, long index, T element);

        /// <summary>
        /// 保留列表中指定范围内的元素值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        Task LTrimAsync(string key, long start, long stop);

        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> RPopAsync(string key);
        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> RPopAsync<T>(string key);

        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        Task<string> RPopLPushAsync(string source, string destination);
        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        Task<T> RPopLPushAsync<T>(string source, string destination);

        /// <summary>
        /// 在列表中添加一个或多个值到列表尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        Task<long> RPushAsync(string key, params object[] elements);

        /// <summary>
        /// 为已存在的列表添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        Task<long> RPushXAsync(string key, params object[] elements);

        #endregion

        #region 集合（Set）

        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">集合元素</param>
        /// <returns>新增条数</returns>
        Task<long> SAddAsync(string key, params object[] members);
        /// <summary>
        /// 返回集合的大小（元素个数）
        /// </summary>
        /// <param name="key"></param>
        /// <returns>集合大小</returns>
        Task<long> SCardAsync(string key);
        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SDiffAsync(params string[] keys);
        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SDiffAsync<T>(params string[] keys);
        /// <summary>
        /// 将多个集合的差集存储
        /// </summary>
        /// <param name="destination">存储差集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns>差集条数</returns>
        Task<long> SDiffStoreAsync(string destination, params string[] keys);
        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SInterAsync(params string[] keys);
        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SInterAsync<T>(params string[] keys);
        /// <summary>
        /// 将多个集合的交集存储
        /// </summary>
        /// <param name="destination">存储交集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns></returns>
        Task<long> SInterStoreAsync(string destination, params string[] keys);
        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SUnionAsync(params string[] keys);
        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SUnionAsync<T>(params string[] keys);
        /// <summary>
        /// 将多个集合的并集存储
        /// </summary>
        /// <param name="destination">存储并集的key</param>
        /// <param name="keys">多个集合的key</param>
        Task<long> SUnionStoreAsync(string destination, params string[] keys);
        /// <summary>
        /// 对象是否存在集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<bool> SIsMemberAsync<T>(string key, T member);
        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string[]> SMembersAsync(string key);
        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T[]> SMembersAsync<T>(string key);
        /// <summary>
        /// 将集合内的元素移动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源集合key</param>
        /// <param name="destination">目标集合key</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        Task<bool> SMoveAsync<T>(string source, string destination, T member);
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> SPopAsync(string key);
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> SPopAsync<T>(string key);
        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        Task<string[]> SPopAsync(string key, int count);
        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        Task<T[]> SPopAsync<T>(string key, int count);
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> SRandMemberAsync(string key);
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> SRandMemberAsync<T>(string key);
        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        Task<string[]> SRandMemberAsync(string key, int count);
        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        Task<T[]> SRandMemberAsync<T>(string key, int count);
        /// <summary>
        /// 移除集合中的多个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">元素</param>
        /// <returns></returns>
        Task<long> SRemAsync(string key, params object[] members);
        /// <summary>
        /// 返回集合迭代器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<ScanResult<string>> SScanAsync(string key, long cursor, string pattern, long count);
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
