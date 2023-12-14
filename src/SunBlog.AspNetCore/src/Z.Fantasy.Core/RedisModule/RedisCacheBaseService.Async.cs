using FreeRedis;

namespace Z.Fantasy.Core.RedisModule
{
    public partial class RedisCacheBaseService
    {
        #region 普通类

        /// <summary>
        /// 如果key已经存在并且是一个字符串，APPEND 命令将指定的value追加到该key原来值（value）的末尾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public async Task<long> AppendAsync(string key, string value) => await _redisClient.AppendAsync(key, value);

        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key) => await _redisClient.GetAsync(key);
        /// <summary>
        /// 获取指定key的值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) => await _redisClient.GetAsync<T>(key);

        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<string> GetSetAsync(string key, string value) => await _redisClient.GetSetAsync(key, value);
        /// <summary>
        /// 将给定key的值设为 value ，并返回key的旧值(old value)
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<string> GetSetAsync<T>(string key, T value) => await _redisClient.GetSetAsync(key, value);

        /// <summary>
        /// 用于在key存在时删除 key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<long> DelAsync(params string[] keys) => await _redisClient.DelAsync(keys);

        /// <summary>
        /// 检查给定key是否存在
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<bool>  ExistsAsync(string key) => await _redisClient.ExistsAsync(key);

        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        public async Task<bool>  ExpireAsync(string key, int seconds) => await _redisClient.ExpireAsync(key, seconds);
        /// <summary>
        /// 为给定key设置过期时间
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public async Task<bool>  ExpireAtAsync(string key, DateTime expire) => await _redisClient.ExpireAtAsync(key, expire);

        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public async Task<long> IncrByAsync(string key, long value = 1) => await _redisClient.IncrByAsync(key, value);
        /// <summary>
        /// 为key所储存的值加上给定的浮点增量值increment
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        public async Task<decimal>  IncrByFloatAsync(string key, decimal value) => await _redisClient.IncrByFloatAsync(key, value);

        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        public async Task<string[]>  KeysAsync(string pattern) => await _redisClient.KeysAsync($"{cacheOptions.KeyPrefix}:" + pattern?.Replace($"{cacheOptions.KeyPrefix}:", ""));


        /// <summary>
        /// 修改key的名称
        /// </summary>
        /// <param name="key"> 旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        public async Task RenameAsync(string key, string newKey) => await _redisClient.RenameAsync(key, newKey);

        /// <summary>
        /// 设置指定key的值，所有写入参数object都支持string | byte[]| 数值 | 对象
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">值</param>
        /// <param name="timeoutSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        public async Task  SetAsync(string key, string value, int timeoutSeconds = -1) => await _redisClient.SetAsync(key, value, timeoutSeconds);

        /// <summary>
        /// 设置指定key的值，所有写入参数object都支持string | byte[]| 数值 | 对象
        /// </summary>
        /// <typeparam name="TData">消息对象类型</typeparam>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="data">实体对象</param>
        /// <param name="timeoutSeconds">过期时间（单位：秒）</param>
        /// <returns></returns>
        public async Task  SetAsync<TData>(string key, TData data, int timeoutSeconds = 0) => await _redisClient.SetAsync(key, data, timeoutSeconds);

        /// <summary>
        /// 返回所有给定键的值，对于其中个别键不存在，或其值不为字符串的，返回特殊的 nil 值，因此 MGET 指令永远不会执行失败
        /// </summary>
        /// <param name="keys">如：runoob*</param>
        /// <returns></returns>
        public async Task<string[]>  MGetAsync(params string[] keys) => await _redisClient.MGetAsync(keys);
        /// <summary>
        /// 将给定键的值设置为对应的新值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="value">键值</param>
        /// <param name="keyValues">其它键值对</param>
        /// <returns></returns>
        public async Task  MSetAsync(string key, object value, params object[] keyValues) => await _redisClient.MSetAsync(key, value, keyValues);

        /// <summary>
        /// 返回键的剩余生存时间
        /// </summary>
        /// <param name="key"> 旧名称，不含prefix前辍</param>
        public async Task<long> TtlAsync(string key) => await _redisClient.TtlAsync(key);

        #endregion

        #region 哈希表

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public async Task<long> HDelAsync(string key, params string[] fields) => await _redisClient.HDelAsync(key, fields);

        /// <summary>
        /// 查看哈希表key中，指定的字段是否存在
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<bool>  HExistsAsync(string key, string field) => await _redisClient.HExistsAsync(key, field);

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<string> HGetAsync(string key, string field) => await _redisClient.HGetAsync(key, field);
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<T> HGetAsync<T>(string key, string field) => await _redisClient.HGetAsync<T>(key, field);

        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>>  HGetAllAsync(string key) => await _redisClient.HGetAllAsync(key);
        /// <summary>
        /// 获取在哈希表中指定key的所有字段和值
        /// </summary>
        /// <typeparam name="T">byte[]或其他类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> HGetAllAsync<T>(string key) => await _redisClient.HGetAllAsync<T>(key);

        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量v
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public async Task<long> HIncrByAsync(string key, string field, long value = 1) => await _redisClient.HIncrByAsync(key, field, value);
        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量increment
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值</param>
        /// <returns></returns>
        public async Task<decimal>  HIncrByFloatAsync(string key, string field, decimal value) => await _redisClient.HIncrByFloatAsync(key, field, value);

        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<string[]>  HKeysAsync(string key) => await _redisClient.HKeysAsync(key);

        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<long> HLenAsync(string key) => await _redisClient.HLenAsync(key);

        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public async Task<string[]>  HMGetAsync(string key, params string[] fields) => await _redisClient.HMGetAsync(key, fields);
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public async Task<T[]>  HMGetAsync<T>(string key, params string[] fields) => await _redisClient.HMGetAsync<T>(key, fields);

        /// <summary>
        /// 将哈希表key中的字段field的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<long> HSetAsync(string key, string field, string value) => await _redisClient.HSetAsync(key, field, value);
        /// <summary>
        /// 将哈希表key中的字段field的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public async Task<long> HSetAsync<T>(string key, Dictionary<string, T> keyValues) => await _redisClient.HSetAsync<T>(key, keyValues);

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public async Task  HMSetAsync<T>(string key, string field, T value, params object[] keyValues) => await _redisClient.HMSetAsync(key, field, value, keyValues);

        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool>  HSetNxAsync(string key, string field, string value) => await _redisClient.HSetNxAsync(key, field, value);

        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<string[]>  HValsAsync(string key) => await _redisClient.HValsAsync(key);
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">键名（不含prefix前辍）</param>
        /// <returns></returns>
        public async Task<T[]>  HValsAsync<T>(string key) => await _redisClient.HValsAsync<T>(key);

        #endregion

        #region 列表（List）

        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<string> BLPopAsync(string key, int timeoutSeconds) => await _redisClient.BLPopAsync(key, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<T> BLPopAsync<T>(string key, int timeoutSeconds) => await _redisClient.BLPopAsync<T>(key, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns><param name="timeoutSeconds">超时时间（秒）</param>
        public async Task<KeyValue<string>> BLPopAsync(string[] keys, int timeoutSeconds) => await _redisClient.BLPopAsync(keys, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的第一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<KeyValue<T>> BLPopAsync<T>(string[] keys, int timeoutSeconds) => await _redisClient.BLPopAsync<T>(keys, timeoutSeconds);

        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<string> BRPopAsync(string key, int timeoutSeconds) => await _redisClient.BRPopAsync(key, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<T> BRPopAsync<T>(string key, int timeoutSeconds) => await _redisClient.BRPopAsync<T>(key, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<KeyValue<string>> BRPopAsync(string[] keys, int timeoutSeconds) => await _redisClient.BRPopAsync(keys, timeoutSeconds);
        /// <summary>
        /// 移出并获取列表的最后一个元素（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<KeyValue<T>> BRPopAsync<T>(string[] keys, int timeoutSeconds) => await _redisClient.BRPopAsync<T>(keys, timeoutSeconds);

        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<string> BRPopLPushAsync(string source, string destination, int timeoutSeconds) => await _redisClient.BRPopLPushAsync(source, destination, timeoutSeconds);
        /// <summary>
        /// 从列表中取出最后一个元素，并插入到另外一个列表的头部（如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <returns></returns>
        public async Task<T> BRPopLPushAsync<T>(string source, string destination, int timeoutSeconds) => await _redisClient.BRPopLPushAsync<T>(source, destination, timeoutSeconds);

        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public async Task<string> LIndexAsync(string key, long index) => await _redisClient.LIndexAsync(key, index);
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public async Task<T> LIndexAsync<T>(string key, long index) => await _redisClient.LIndexAsync<T>(key, index);

        /// <summary>
        /// 指定列表中一个元素在它之前或之后插入另外一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="direction">插入方向（before|after）</param>
        /// <param name="pivot">参照元素</param>
        /// <param name="element">待插入的元素</param>
        /// <returns></returns>
        public async Task<long> LInsertAsync(string key, InsertDirection direction, object pivot, object element) => await _redisClient.LInsertAsync(key, direction, pivot, element);

        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> LLenAsync(string key) => await _redisClient.LLenAsync(key);

        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> LPopAsync(string key) => await _redisClient.LPopAsync(key);
        /// <summary>
        /// 从列表的头部弹出元素，默认为第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> LPopAsync<T>(string key) => await _redisClient.LPopAsync<T>(key);

        /// <summary>
        /// 获取列表 key 中匹配给定 element 元素的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="element">元素</param>
        /// <param name="rank">从第几个匹配开始计算</param>
        /// <returns></returns>
        public async Task<long> LPosAsync<T>(string key, T element, int rank = 0) => await _redisClient.LPosAsync<T>(key, element, rank);
        /// <summary>
        /// 获取列表 key 中匹配给定 element 元素的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="element">元素</param>
        /// <param name="rank">从第几个匹配开始计算</param>
        /// <param name="count">要匹配的总数</param>
        /// <param name="maxLen">只查找最多 len 个元素</param>
        /// <returns></returns>
        public async Task<long[]> LPosAsync<T>(string key, T element, int rank, int count, int maxLen) => await _redisClient.LPosAsync<T>(key, element, rank, count, maxLen);

        /// <summary>
        /// 在列表头部插入一个或者多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        public async Task<long> LPushAsync(string key, params object[] elements) => await _redisClient.LPushAsync(key, elements);

        /// <summary>
        /// 当储存列表的 key 存在时，用于将值插入到列表头部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        public async Task<long> LPushXAsync(string key, params object[] elements) => await _redisClient.LPushXAsync(key, elements);

        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        public async Task<string[]>  LRangeAsync(string key, long start, long stop) => await _redisClient.LRangeAsync(key, start, stop);
        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        /// <returns></returns>
        public async Task<T[]>  LRangeAsync<T>(string key, long start, long stop) => await _redisClient.LRangeAsync<T>(key, start, stop);

        /// <summary>
        /// 从列表中删除元素与 value 相等的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count">删除的数量（等于0时全部移除，小于0时从表尾开始向表头搜索，大于0时从表头开始向表尾搜索）</param>
        /// <param name="element">待删除的元素</param>
        /// <returns></returns>
        public async Task<long> LRemAsync<T>(string key, long count, T element) => await _redisClient.LRemAsync<T>(key, count, element);

        /// <summary>
        /// 通过其索引设置列表中元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <param name="element">元素</param>
        public async Task  LSetAsync<T>(string key, long index, T element) => await _redisClient.LSetAsync<T>(key, index, element);

        /// <summary>
        /// 保留列表中指定范围内的元素值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始偏移量</param>
        /// <param name="stop">结束偏移量</param>
        public async Task  LTrimAsync(string key, long start, long stop) => await _redisClient.LTrimAsync(key, start, stop);

        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> RPopAsync(string key) => await _redisClient.RPopAsync(key);
        /// <summary>
        /// 移除列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> RPopAsync<T>(string key) => await _redisClient.RPopAsync<T>(key);

        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        public async Task<string> RPopLPushAsync(string source, string destination) => await _redisClient.RPopLPushAsync(source, destination);
        /// <summary>
        /// 移除列表的最后一个元素，并将该元素添加到另一个列表并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源列表</param>
        /// <param name="destination">目标列表</param>
        /// <returns></returns>
        public async Task<T> RPopLPushAsync<T>(string source, string destination) => await _redisClient.RPopLPushAsync<T>(source, destination);

        /// <summary>
        /// 在列表中添加一个或多个值到列表尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        public async Task<long> RPushAsync(string key, params object[] elements) => await _redisClient.RPushAsync(key, elements);

        /// <summary>
        /// 为已存在的列表添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="elements">元素数组</param>
        /// <returns></returns>
        public async Task<long> RPushXAsync(string key, params object[] elements) => await _redisClient.RPushXAsync(key, elements);

        #endregion

        #region 集合（Set）

        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">集合元素</param>
        /// <returns>新增条数</returns>
        public async Task<long> SAddAsync(string key, params object[] members) => await _redisClient.SAddAsync(key, members);

        /// <summary>
        /// 返回集合的大小（元素个数）
        /// </summary>
        /// <param name="key"></param>
        /// <returns>集合大小</returns>
        public async Task<long> SCardAsync(string key) => await _redisClient.SCardAsync(key);

        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<string[]>  SDiffAsync(params string[] keys) => await _redisClient.SDiffAsync(keys);
        /// <summary>
        /// 返回多个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<T[]>  SDiffAsync<T>(params string[] keys) => await _redisClient.SDiffAsync<T>(keys);

        /// <summary>
        /// 将多个集合的差集存储
        /// </summary>
        /// <param name="destination">存储差集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns>差集条数</returns>
        public async Task<long> SDiffStoreAsync(string destination, params string[] keys) => await _redisClient.SDiffStoreAsync(destination, keys);

        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<string[]>  SInterAsync(params string[] keys) => await _redisClient.SInterAsync(keys);

        /// <summary>
        /// 返回多个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<T[]>  SInterAsync<T>(params string[] keys) => await _redisClient.SInterAsync<T>(keys);

        /// <summary>
        /// 将多个集合的交集存储
        /// </summary>
        /// <param name="destination">存储交集的key</param>
        /// <param name="keys">多个集合的key</param>
        /// <returns></returns>
        public async Task<long> SInterStoreAsync(string destination, params string[] keys) => await _redisClient.SInterStoreAsync(destination, keys);

        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<string[]>  SUnionAsync(params string[] keys) => await _redisClient.SUnionAsync(keys);

        /// <summary>
        /// 返回多个集合的并集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<T[]>  SUnionAsync<T>(params string[] keys) => await _redisClient.SUnionAsync<T>(keys);

        /// <summary>
        /// 将多个集合的并集存储
        /// </summary>
        /// <param name="destination">存储并集的key</param>
        /// <param name="keys">多个集合的key</param>
        public async Task<long> SUnionStoreAsync(string destination, params string[] keys) => await _redisClient.SUnionStoreAsync(destination, keys);

        /// <summary>
        /// 对象是否存在集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<bool>  SIsMemberAsync<T>(string key, T member) => await _redisClient.SIsMemberAsync(key, member);

        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]>  SMembersAsync(string key) => await _redisClient.SMembersAsync(key);

        /// <summary>
        /// 集合内所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T[]>  SMembersAsync<T>(string key) => await _redisClient.SMembersAsync<T>(key);

        /// <summary>
        /// 将集合内的元素移动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源集合key</param>
        /// <param name="destination">目标集合key</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        public async Task<bool>  SMoveAsync<T>(string source, string destination, T member) => await _redisClient.SMoveAsync(source, destination, member);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> SPopAsync(string key) => await _redisClient.SPopAsync(key);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> SPopAsync<T>(string key) => await _redisClient.SPopAsync<T>(key);

        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        public async Task<string[]>  SPopAsync(string key, int count) => await _redisClient.SPopAsync(key, count);

        /// <summary>
        /// 移除并返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">移除个数</param>
        /// <returns></returns>
        public async Task<T[]>  SPopAsync<T>(string key, int count) => await _redisClient.SPopAsync<T>(key, count);

        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> SRandMemberAsync(string key) => await _redisClient.SRandMemberAsync(key);

        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> SRandMemberAsync<T>(string key) => await _redisClient.SRandMemberAsync<T>(key);

        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        public async Task<string[]>  SRandMemberAsync(string key, int count) => await _redisClient.SRandMemberAsync(key, count);

        /// <summary>
        /// 返回集合中的多个随机元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">返回个数</param>
        public async Task<T[]>  SRandMemberAsync<T>(string key, int count) => await _redisClient.SRandMemberAsync<T>(key, count);

        /// <summary>
        /// 移除集合中的多个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members">元素</param>
        /// <returns></returns>
        public async Task<long> SRemAsync(string key, params object[] members) => await _redisClient.SRemAsync(key, members);

        /// <summary>
        /// 返回集合迭代器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<ScanResult<string>> SScanAsync(string key, long cursor, string pattern, long count) => await _redisClient.SScanAsync(key, cursor, pattern, count);

        #endregion

        #region 有序集合（Sorted Set）

        /// <summary>
        /// 获取有序集合的长度
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="min">最小值（负无穷）</param>
        /// <param name="max">最大值（正无穷）</param>
        /// <returns></returns>
        public async Task<long> ZCountAsync(string key, decimal min = decimal.MinValue, decimal max = decimal.MaxValue) => await _redisClient.ZCountAsync(key, min, max);
        /// <summary>
        /// 获取有序集合的长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public async Task<long> ZCountAsync(string key, string min, string max) => await _redisClient.ZCountAsync(key, min, max);

        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素</param>
        /// <returns></returns>
        public async Task<long> ZAddAsync(string key, decimal score, string member, params object[] scoreMembers) => await _redisClient.ZAddAsync(key, score, member, scoreMembers);
        /// <summary>
        /// 向有序集合插入元素
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="scoreMembers">元素数组</param>
        /// <returns></returns>
        public async Task<long> ZAddAsync(string key, ZMember[] scoreMembers) => await _redisClient.ZAddAsync(key, scoreMembers);

        /// <summary>
        /// 移除有序集合指定元素
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="members">元素数组</param>
        /// <returns></returns>
        public async Task<long> ZRemAsync(string key, params string[] members) => members?.Length == 0 || ! (await _redisClient.ExistsAsync(key)) ? 0 : _redisClient.ZRem(key, members);

        /// <summary>
        /// 获取有序集合指定元素的排名顺序
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        public async Task<long?> ZRankAsync(string key, string member) => await _redisClient.ZRankAsync(key, member);

        /// <summary>
        /// 获取有序集合指定元素的指定区间内的元素
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <returns></returns>
        public async Task<string[]>  ZRangeAsync(string key, decimal start = 0, decimal stop = -1) => await _redisClient.ExistsAsync(key) ? _redisClient.ZRange(key, start, stop) : null;

        /// <summary>
        /// 获取元素个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ZCardAsync(string key) => await _redisClient.ZCardAsync(key);

        /// <summary>
        /// 只有在member不存在时，向有序集合插入元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素数组</param>
        /// <returns></returns>
        public async Task<long> ZAddNxAsync(string key, decimal score, string member, params object[] scoreMembers) => await _redisClient.ZAddNxAsync(key, score, member, scoreMembers);
        /// <summary>
        /// 只有在member不存在时，向有序集合插入元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers">元素分数对象数组</param>
        /// <param name="than"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public async Task<long> ZAddNxAsync(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false) => await _redisClient.ZAddNxAsync(key, scoreMembers, than, ch);
        /// <summary>
        /// 只有在member存在时，更新有序集合元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score">分数</param>
        /// <param name="member">元素</param>
        /// <param name="scoreMembers">元素数组</param>
        /// <returns></returns>
        public async Task<long> ZAddXxAsync(string key, decimal score, string member, params object[] scoreMembers) => await _redisClient.ZAddXxAsync(key, score, member, scoreMembers);
        /// <summary>
        /// 只有在member存在时，更新有序集合元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers">元素数组</param>
        /// <param name="than"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public async Task<long> ZAddXxAsync(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false) => await _redisClient.ZAddXxAsync(key, scoreMembers, than, ch);

        /// <summary>
        /// 有序集合中对指定元素的分数加上增量 increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment">增量（默认为1）</param>
        /// <param name="member">元素</param>
        /// <returns></returns>
        public async Task<decimal>  ZIncrByAsync(string key, decimal increment, string member) => await _redisClient.ZIncrByAsync(key, increment, member);

        /// <summary>
        /// 计算 numkeys 个有序集合的交集，并且把结果放到 destination 中
        /// </summary>
        /// <param name="destination">目标key</param>
        /// <param name="keys"></param>
        /// <param name="weights">乘法因子（默认为1）</param>
        /// <param name="aggregate">结果集的聚合方式（默认为SUM）</param>
        /// <returns></returns>
        public async Task<long> ZInterStoreAsync(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null) => await _redisClient.ZInterStoreAsync(destination, keys, weights, aggregate);

        /// <summary>
        /// 在有序集合中计算指定字典区间内元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public async Task<long> ZLexCountAsync(string key, string min, string max) => await _redisClient.ZLexCountAsync(key, min, max);

        /// <summary>
        /// 删除并返回最多count个有序集合key中最低得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ZMember>  ZPopMinAsync(string key) => await _redisClient.ZPopMinAsync(key);
        /// <summary>
        /// 删除并返回最多count个有序集合key中最低得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<ZMember[]>  ZPopMinAsync(string key, int count) => await _redisClient.ZPopMinAsync(key, count);

        /// <summary>
        /// 删除并返回最多count个有序集合key中的最高得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ZMember>  ZPopMaxAsync(string key) => await _redisClient.ZPopMaxAsync(key);
        /// <summary>
        /// 删除并返回最多count个有序集合key中的最高得分的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<ZMember[]>  ZPopMaxAsync(string key, int count) => await _redisClient.ZPopMaxAsync(key, count);

        /// <summary>
        /// 删除成员名称按字典由低到高排序介于min 和 max 之间的所有成员（集合中所有成员的分数相同）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<long> ZRemRangeByLexAsync(string key, string min, string max) => await _redisClient.ZRemRangeByLexAsync(key, min, max);

        /// <summary>
        /// 移除有序集key中，指定排名(rank)区间 start 和 stop 内的所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<long> ZRemRangeByRankAsync(string key, long start, long stop) => await _redisClient.ZRemRangeByRankAsync(key, start, stop);

        /// <summary>
        /// 移除有序集key中，所有score值介于min和max之间(包括等于min或max)的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max) => await _redisClient.ZRemRangeByScoreAsync(key, min, max);
        /// <summary>
        /// 移除有序集key中，所有score值介于min和max之间(包括等于min或max)的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<long> ZRemRangeByScoreAsync(string key, string min, string max) => await _redisClient.ZRemRangeByScoreAsync(key, min, max);

        /// <summary>
        /// 获取有序集key中，指定区间内的成员（成员的位置按score值递减(从高到低)来排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<string[]>  ZRevRangeAsync(string key, decimal start, decimal stop) => await _redisClient.ZRevRangeAsync(key, start, stop);

        /// <summary>
        /// 获取有序集key中，指定区间内的成员+分数列表（成员的位置按score值递减(从高到低)来排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<ZMember[]>  ZRevRangeWithScoresAsync(string key, decimal start, decimal stop) => await _redisClient.ZRevRangeWithScoresAsync(key, start, stop);

        /// <summary>
        /// 按字典从低到高排序，取索引范围内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]>  ZRevRangeByLexAsync(string key, decimal max, decimal min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByLexAsync(key, max, min, offset, count);
        /// <summary>
        /// 按字典从低到高排序，取索引范围内的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]>  ZRevRangeByLexAsync(string key, string max, string min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByLexAsync(key, max, min, offset, count);

        /// <summary>
        /// 获取有序集合中指定分数区间的成员列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]>  ZRevRangeByScoreAsync(string key, decimal max, decimal min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByScoreAsync(key, max, min, offset, count);
        /// <summary>
        /// 获取有序集合中指定分数区间的成员列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]>  ZRevRangeByScoreAsync(string key, string max, string min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByScoreAsync(key, max, min, offset, count);

        /// <summary>
        /// 获取有序集合中指定分数区间的成员+分数列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<ZMember[]>  ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByScoreWithScoresAsync(key, max, min, offset, count);
        /// <summary>
        /// 获取有序集合中指定分数区间的成员+分数列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<ZMember[]>  ZRevRangeByScoreWithScoresAsync(string key, string max, string min, int offset = 0, int count = 0) => await _redisClient.ZRevRangeByScoreWithScoresAsync(key, max, min, offset, count);

        /// <summary>
        /// 获取有序集key中成员member的排名（按score值从高到低排列）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<long> ZRevRankAsync(string key, string member) => await _redisClient.ZRevRankAsync(key, member);

        /// <summary>
        /// 获取有序集key成员 member 的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<decimal?> ZScoreAsync(string key, string member) => await _redisClient.ZScoreAsync(key, member);

        /// <summary>
        /// 计算一个或多个有序集的并集，并存储在新的 key 中
        /// </summary>
        /// <param name="destination">目标key</param>
        /// <param name="keys"></param>
        /// <param name="weights">乘法因子（默认为1）</param>
        /// <param name="aggregate">结果集的聚合方式（默认为SUM）</param>
        /// <returns></returns>
        public async Task<long> ZUnionStoreAsync(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null) => await _redisClient.ZUnionStoreAsync(destination, keys, weights, aggregate);


        #endregion
    }
}
