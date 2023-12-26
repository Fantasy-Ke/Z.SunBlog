namespace Z.RabbitMQ;

/// <summary>
/// 负载均衡配置器
/// </summary>
public class RabbitMQConnectionLoadBalancing
{
    public RabbitMQConnectionLoadBalancing(int queueCount, string queueName)
    {
        MaxSize = queueCount;
        QueueName = queueName;
        Initialize();
    }

    /// <summary>
    /// 队列名称
    /// </summary>
    public string QueueName { get; set; }

    /// <summary>
    /// 当前负载均衡的索引
    /// </summary>
    protected int _currentIndex;

    /// <summary>
    /// 所有的负载均衡配置
    /// </summary>
    protected virtual ServerConfig[] ServerConfigs { get; set; }

    /// <summary>
    /// 最大数量
    /// </summary>
    public virtual int MaxSize { get; set; }

    /// <summary>
    /// 是否已初始化成功
    /// </summary>
    public bool Initialized { get; protected set; }

    public string NextKey()
    {
        if (Initialized == false)
        {
            Initialize();
        }

        _currentIndex = NextIndex();
        return ServerConfigs[_currentIndex].Name;
    }

    public int NextIndex()
    {
        if (Initialized == false)
        {
            Initialize();
        }

        return NextServerIndex(ServerConfigs);
    }

    /// <summary>
    /// 获取所有负载均衡的(topic/队列)名称
    /// </summary>
    /// <returns></returns>
    public List<string> GetAll()
    {
        return ServerConfigs.Select(o => o.Name).ToList();
    }

    /// <summary>
    /// 下一个负载的索引
    /// </summary>
    /// <param name="serverConfigArray"></param>
    /// <returns></returns>
    public static int NextServerIndex(ServerConfig[] serverConfigArray)
    {
        var index = -1;
        var total = 0;
        var size = serverConfigArray.Count();
        for (var i = 0; i < size; i++)
        {
            serverConfigArray[i].Current += serverConfigArray[i].Weight;
            total += serverConfigArray[i].Weight;
            if (index == -1 || serverConfigArray[index].Current < serverConfigArray[i].Current)
            {
                index = i;
            }
        }
        serverConfigArray[index].Current -= total;
        return index;
    }

    #region Private
    private void Initialize()
    {
        ServerConfigs = new ServerConfig[MaxSize];
        for (var i = 0; i < MaxSize; i++)
        {
            ServerConfigs[i] = new ServerConfig() { Weight = 1, Name = $"{QueueName}_{i}" };
        }
        Initialized = true;
    }
    #endregion
}

/// <summary>
/// 负载的配置
/// </summary>
public struct ServerConfig
{
    //初始权重
    public int Weight { get; set; }

    //当前权重
    public int Current { get; set; }

    //服务名称
    public string Name { get; set; }
}
