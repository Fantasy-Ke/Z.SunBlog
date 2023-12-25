using RabbitMQ.Client;

namespace Z.RabbitMQ;

/// <summary>
/// 订阅参数定义
/// </summary>
public class RabbitSubscribeDef
{
    /// <summary>
    /// 消费者初始化器类型
    /// </summary>
    public Type RabbitConsumerInitializerType { get; set; }

    /// <summary>
    /// 消费者使用的配置名称
    /// </summary>
    public string ConfigName { get; set; }

    /// <summary>
    /// 消费者使用的队列名称
    /// </summary>
    public string QueueName { get; set; }

    /// <summary>
    /// 队列数，默认为1
    /// </summary>
    public int QueueCount { get; set; }

    /// <summary>
    /// 使用的消息通道
    /// </summary>
    public IModel Channel { get; set; }

    /// <summary>
    /// 消息最大优先级
    /// </summary>

    public int XMaxPriority { get; set; }

    /// <summary>
    /// 是否开启负载均衡
    /// </summary>
    public bool IsLoadBalancing => QueueCount > 1;
    /// <summary>
    /// 是否为死信队列
    /// </summary>
    public bool IsDLXQueue { get; set; }

    /// <summary>
    /// 负载均衡配置
    /// </summary>
    public RabbitMQConnectionLoadBalancing LoadBalancing { get; set; }

    public void InitLoadBalancing()
    {
        LoadBalancing = new RabbitMQConnectionLoadBalancing(QueueCount, QueueName);
    }
}
