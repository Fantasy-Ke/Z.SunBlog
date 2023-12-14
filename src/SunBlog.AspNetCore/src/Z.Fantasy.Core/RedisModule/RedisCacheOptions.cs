namespace Z.Fantasy.Core.RedisModule;

public class RedisCacheOptions
{
    public string Configuration { get; set; }

    public string KeyPrefix { get; set; } 

    public bool Enable { get; set; }

    public SideCaching SideCache { get; set; }
}

public class SideCaching
{
    public bool Enable { get; set; }
    /// <summary>
    /// 容量
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// 需要本地缓存的key
    /// </summary>
    public string KeyFilterCache { get; set; }

    /// <summary>
    /// 本地长期未使用的
    /// </summary>
    public int ExpiredMinutes { get; set; }
}
