namespace Z.RabbitMQ;

public static class RabbitConsts
{
    /// <summary>
    /// 默认使用的配置名称
    /// </summary>
    public static string DefaultConfigName { get; } = "Default";
    /// <summary>
    /// 死信队列前缀
    /// </summary>
    public const string DLXQueuePrefix = "DLX.";
}
