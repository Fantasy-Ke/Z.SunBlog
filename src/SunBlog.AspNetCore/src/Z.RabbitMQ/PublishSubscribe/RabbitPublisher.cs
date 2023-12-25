using RabbitMQ.Client;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.PublishSubscribe;

/// <summary>
/// 发布者
/// </summary>
public interface IRabbitPublisher
{
    /// <summary>
    /// 发布消息
    /// </summary>
    /// <typeparam name="TEventData">消息的数据类型，需与订阅时一致</typeparam>
    /// <param name="configName">配置名称</param>
    /// <param name="queueName">队列名称（事件名称）</param>
    /// <param name="eventData">消息数据</param>
    /// <param name="priority">消息优先级，越大越优先</param>
    /// <param name="queueCount">队列数，需与订阅时一致</param>
    /// <param name="xMaxPriority">最大优先级，需与订阅时一致</param>
    void Publish<TRabbitConsumer, TEventData>
        (string configName, string queueName, TEventData eventData, int priority = 0, int queueCount = 1, int xMaxPriority = 0)
        where TRabbitConsumer : IRabbitConsumer<TEventData>, IRabbitConsumerInitializer;
}

public class RabbitPublisher : IRabbitPublisher
{
    protected readonly IRabbitConnectionStore _rabbitConnectionStore;
    protected readonly IMsgPackSerializer _msgPackSerializer;
    protected readonly RabbitSubscriber _rabbitSubscriber;
    public RabbitPublisher(IRabbitConnectionStore rabbitConnectionStore, IMsgPackSerializer msgPackSerializer, RabbitSubscriber rabbitSubscriber)
    {
        _rabbitConnectionStore = rabbitConnectionStore;
        _msgPackSerializer = msgPackSerializer;
        _rabbitSubscriber = rabbitSubscriber;
    }

    public void Publish<TRabbitConsumer, TEventData>
        (string configName, string queueName, TEventData eventData, int priority = 0, int queueCount = 1, int xMaxPriority = 0)
        where TRabbitConsumer : IRabbitConsumer<TEventData>, IRabbitConsumerInitializer
    {
        configName = GetConfigName(configName);

        // 消息数据
        var buffer = _msgPackSerializer.MessageToBytes(eventData);

        // 获取连接，这里永远都是最新的
        var connection = _rabbitConnectionStore.GetConnection(configName);

        // 获取订阅通道
        var channel = _rabbitConnectionStore.GetChannel(connection, queueName);

        // 获取订阅者
        var subscribeDefDict = _rabbitConnectionStore.GetOrAddQueueSubscribeDef(connection);
        subscribeDefDict.TryGetValue(queueName, out var subscribeDef);

        // 如果在发布之前没有先订阅，则会在此处初始化存储并创建队列，但不绑定消费者
        if (subscribeDef == null)
        {
            subscribeDef = new RabbitSubscribeDef
            {
                RabbitConsumerInitializerType = typeof(TRabbitConsumer),
                ConfigName = configName,
                QueueName = queueName,
                QueueCount = queueCount,
                XMaxPriority = xMaxPriority,
                IsDLXQueue = false
            };
            subscribeDefDict.TryAdd(queueName, subscribeDef);

            var sourceQueueName = subscribeDef.QueueName;
            if (subscribeDef.IsLoadBalancing)
            {
                subscribeDef.InitLoadBalancing();
                foreach (var queueNameItem in subscribeDef.LoadBalancing.GetAll())
                {
                    CreateQueue(sourceQueueName, queueNameItem, channel, subscribeDef.XMaxPriority);
                }
            }
            else
            {
                CreateQueue(sourceQueueName, sourceQueueName, channel, subscribeDef.XMaxPriority);
            }
        }

        // 负载均衡发布
        if (subscribeDef.IsLoadBalancing)
        {
            Publish(channel, subscribeDef.LoadBalancing.NextKey(), buffer, priority);
            return;
        }

        Publish(channel, queueName, buffer, priority);

    }

    #region 内部方法
    /// <summary>
    /// 创建队列
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="channel"></param>
    private void CreateQueue(string sourceQueueName, string queueName, IModel channel, int xMaxPriority = 0)
    {
        //死信交换机
        var dlxexChange = RabbitConsts.DLXQueuePrefix + sourceQueueName;
        //死信队列
        var dlxQueueName = dlxexChange;

        // 交换机名称
        var exchangeName = queueName;
        //定义一个Fanout类型交换机
        channel.ExchangeDeclare(queueName, ExchangeType.Fanout, false, false, null);
        // 定义队列参数
        var args = new Dictionary<string, object> {
            { "x-max-priority", xMaxPriority }, // 定义消息最大优先级 设置为0则代表不使用消息优先级
            { "x-dead-letter-exchange",dlxexChange}, //设置当前队列的DLX(死信交换机)
            { "x-dead-letter-routing-key",dlxQueueName}, //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
            //{ "x-message-ttl",5000} //设置队列的消息过期时间
        };
        // 定义队列
        channel.QueueDeclare(queue: queueName,
                  durable: true,
                  exclusive: false,
                  autoDelete: false,
                  arguments: args);

        channel.QueueBind(queueName, exchangeName, queueName, null);

        // 设置公平调度
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }
    protected virtual void Publish(IModel channel, string queueName, byte[] body, int priority)
    {
        var exchangeName = queueName;

        channel.ExchangeDeclare(exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: false,
            autoDelete: false,
            arguments: null);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        //设置消息的优先级
        properties.Priority = (byte)priority;

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: queueName,
            basicProperties: properties,
            body: body);
    }

    /// <summary>
    /// 获取配置名称
    /// </summary>
    /// <param name="configName"></param>
    /// <returns></returns>
    protected virtual string GetConfigName(string configName)
    {
        if (string.IsNullOrWhiteSpace(configName))
        {
            return RabbitConsts.DefaultConfigName;
        }

        return configName;
    }
    #endregion
}
