using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.Manager;

public class RabbitEventManager : IRabbitEventManager
{
    protected readonly ConcurrentDictionary<IConnection, string> _connectionShutdownDict;
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IRabbitConnectionStore _rabbitConnectionStore;
    protected readonly IRabbitPolicyStore _rabbitPolicyStore;
    protected readonly IMsgPackTransmit _msgPackTransmit;

    public RabbitEventManager(
        ConcurrentDictionary<IConnection, string> connectionShutdownDict,
        IServiceProvider serviceProvider,
        IRabbitConnectionStore rabbitConnectionStore,
        IRabbitPolicyStore rabbitPolicyStore,
        IMsgPackTransmit msgPackTransmit
    )
    {
        _connectionShutdownDict = connectionShutdownDict;
        _serviceProvider = serviceProvider;
        _rabbitConnectionStore = rabbitConnectionStore;
        _rabbitPolicyStore = rabbitPolicyStore;
        _msgPackTransmit = msgPackTransmit;
    }

    #region 发布

    public void Publish<TRabbitConsumer, TEventData>(
        string queueName,
        TEventData eventData,
        string configName = "",
        int priority = 0,
        int queueCount = 1,
        int xMaxPriority = 0
    )
        where TRabbitConsumer : IRabbitConsumer<TEventData>, IRabbitConsumerInitializer
    {
        configName = GetConfigName(configName);

        // 消息数据
        var buffer = _msgPackTransmit.MessageToBytes(eventData);

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
    private void CreateQueue(
        string sourceQueueName,
        string queueName,
        IModel channel,
        int xMaxPriority = 0
    )
    {
        //死信交换机
        var dlxexChange = RabbitConsts.DLXQueuePrefix + sourceQueueName;
        //死信队列
        var dlxQueueName = dlxexChange;

        // 交换机名称
        var exchangeName = queueName;
        //定义一个Fanout类型交换机
        channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, false, false, null);
        // 定义队列参数
        var args = new Dictionary<string, object>
        {
            { "x-max-priority", xMaxPriority }, // 定义消息最大优先级 设置为0则代表不使用消息优先级
            { "x-dead-letter-exchange", dlxexChange }, //设置当前队列的DLX(死信交换机)
            { "x-dead-letter-routing-key", dlxQueueName }, //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
            //{ "x-message-ttl",5000} //设置队列的消息过期时间
        };
        // 定义队列
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args
        );

        channel.QueueBind(queueName, exchangeName, queueName, null);

        // 设置公平调度
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    protected virtual void Publish(IModel channel, string queueName, byte[] body, int priority)
    {
        var exchangeName = queueName;

        //正常交换机通讯
        channel.ExchangeDeclare(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: false,
            autoDelete: false,
            arguments: null
        );

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        //设置消息的优先级
        properties.Priority = (byte)priority;

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: queueName,
            basicProperties: properties,
            body: body
        );
    }
    #endregion

    #endregion

    #region 订阅
    /// <inheritdoc/>
    public void Subscribe<T>(
        string queueName,
        string configName = "",
        int queueCount = 1,
        int xMaxPriority = 0,
        bool isDLX = false
    )
        where T : IRabbitConsumerInitializer
    {
        var retryPolicy = _rabbitPolicyStore.GetRetryPolicy(configName);

        var subscribeDef = new RabbitSubscribeDef
        {
            RabbitConsumerInitializerType = typeof(T),
            ConfigName = configName,
            QueueName = queueName,
            QueueCount = queueCount,
            XMaxPriority = xMaxPriority,
            IsDLXQueue = isDLX
        };

        // 订阅失败重试
        retryPolicy.Execute(() =>
        {
            if (subscribeDef.IsLoadBalancing)
            {
                subscribeDef.InitLoadBalancing();
            }
            // 订阅
            InnerSubscribe(subscribeDef);
        });
    }

    /// <inheritdoc/>
    public void SubscribeDLX<T>(string queueName, string configName = "")
        where T : IRabbitConsumerInitializer
    {
        var dlxQueueName = RabbitConsts.DLXQueuePrefix + queueName;
        Subscribe<T>(configName, dlxQueueName, isDLX: true);
    }

    /// <inheritdoc/>
    public void UnSubscribe<T>(string queueName, string configName = "")
        where T : IRabbitConsumerInitializer
    {
        configName = GetConfigName(configName);
        var connection = _rabbitConnectionStore.GetConnection(configName);

        var dict = _rabbitConnectionStore.GetOrAddQueueSubscribeDef(connection);

        if (!dict.TryRemove(queueName, out var subscribeDef))
        {
            return;
        }
        if (subscribeDef.IsLoadBalancing)
        {
            foreach (var item in subscribeDef.LoadBalancing.GetAll())
            {
                subscribeDef.Channel.ExchangeDelete(item);
                subscribeDef.Channel.QueueDelete(item);
            }
        }
        subscribeDef.Channel.ExchangeDelete(queueName);
        subscribeDef.Channel.QueueDelete(queueName);
        // 释放通道
        subscribeDef.Channel?.Dispose();
    }

    public void Dispose()
    {
        //_connectionRabbitConsumerInitializerDict.Clear();
        _connectionShutdownDict.Clear();
    }

    #region 订阅，内部函数

    /// <summary>
    /// 订阅内部实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rabbitSubscribeDef"></param>
    /// <param name="createNewChannel"></param>
    /// <returns></returns>
    protected virtual void InnerSubscribe(
        RabbitSubscribeDef rabbitSubscribeDef,
        bool createNewChannel = false
    )
    {
        // 获取连接，这里永远都是最新的
        var connection = _rabbitConnectionStore.GetConnection(rabbitSubscribeDef.ConfigName);

        // 获取订阅通道
        var channel = _rabbitConnectionStore.GetChannel(
            connection,
            rabbitSubscribeDef.QueueName,
            createNewChannel
        );
        rabbitSubscribeDef.Channel = channel;

        // 获取消费者实例,并初始化
        var instance =
            _serviceProvider.GetRequiredService(rabbitSubscribeDef.RabbitConsumerInitializerType)
            as IRabbitConsumerInitializer;
        instance.Init(rabbitSubscribeDef);

        // 注册连接断开事件
        RegisterConnectionShutdown(connection, rabbitSubscribeDef);
    }

    /// <summary>
    /// 注册连接断开事件，断开后将把所有的订阅重新连接
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="rabbitSubscribeDef"></param>
    protected virtual void RegisterConnectionShutdown(
        IConnection connection,
        RabbitSubscribeDef rabbitSubscribeDef
    )
    {
        // 添加连接对应的初始化器字典
        _rabbitConnectionStore.GetOrAddQueueSubscribeDef(connection, rabbitSubscribeDef);

        // 判断是否初始化关闭事件，初始化过的话跳过执行
        if (_connectionShutdownDict.ContainsKey(connection))
        {
            return;
        }

        // 加入连接断开字典
        _connectionShutdownDict.TryAdd(connection, rabbitSubscribeDef.ConfigName);

        // 连接断开事件
        connection.ConnectionShutdown += (sender, ea) =>
        {
            // 移除现有连接关联的数据
            _connectionShutdownDict.TryRemove(connection, out var configName);
            var rabbitConsumerInitializerDict = _rabbitConnectionStore.RemoveAndGetIConnection(
                connection
            );

            // 断开连接重试
            var retryPolicy = _rabbitPolicyStore.GetRetryPolicy(configName);
            retryPolicy.Execute(action: () =>
            {
                // 创建新连接
                var connectionNew = _rabbitConnectionStore.GetConnection(configName, true);
                // 重新注册原有消费者
                foreach (var item in rabbitConsumerInitializerDict)
                {
                    // 重新订阅
                    InnerSubscribe(item.Value, true);
                }
            });
        };
    }

    #endregion

    #endregion

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
}
