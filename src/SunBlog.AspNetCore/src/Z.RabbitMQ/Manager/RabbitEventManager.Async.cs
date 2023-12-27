using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Z.RabbitMQ.interfaces;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.Manager;

public partial class RabbitEventManager : IRabbitEventManager
{
    #region 发布

    public async Task PublishAsync<TRabbitConsumer, TEventData>(
        string queueName,
        TEventData eventData,
        string configName = "",
        int priority = 0,
        int queueCount = 1,
        int xMaxPriority = 0,
        CancellationToken cancellationToken = default
    )
        where TRabbitConsumer : IRabbitConsumerAsync<TEventData>, IRabbitConsumerInitializerAsync
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
            //创建默认配置
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
            await Task.Yield();
            return;
        }

        Publish(channel, queueName, buffer, priority);
    }

    #endregion

    #region 订阅
    /// <inheritdoc/>
    public async Task SubscribeAsync<T>(
        string queueName,
        string configName = "",
        int queueCount = 1,
        int xMaxPriority = 0,
        bool isDLX = false,
        CancellationToken cancellationToken = default
    )
        where T : IRabbitConsumerInitializerAsync
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
        await retryPolicy.ExecuteAsync(async () =>
        {
            if (subscribeDef.IsLoadBalancing)
            {
                subscribeDef.InitLoadBalancing();
            }
            // 订阅
            await InnerSubscribeAsync(subscribeDef, cancellationToken: cancellationToken);
        });
    }

    /// <inheritdoc/>
    public async Task SubscribeDLXAsync<T>(
        string queueName,
        string configName = "",
        CancellationToken cancellationToken = default
    )
        where T : IRabbitConsumerInitializerAsync
    {
        var dlxQueueName = RabbitConsts.DLXQueuePrefix + queueName;
        await SubscribeAsync<T>(
            dlxQueueName,
            configName,
            isDLX: true,
            cancellationToken: cancellationToken
        );
    }

    /// <inheritdoc/>
    public async Task UnSubscribeAsync<T>(
        string queueName,
        string configName = "",
        CancellationToken cancellationToken = default
    )
        where T : IRabbitConsumerInitializerAsync
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
        await Task.Yield();
    }

    #region 订阅，内部函数

    /// <summary>
    /// 订阅内部实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rabbitSubscribeDef"></param>
    /// <param name="createNewChannel"></param>
    /// <returns></returns>
    protected virtual async Task InnerSubscribeAsync(
        RabbitSubscribeDef rabbitSubscribeDef,
        CancellationToken cancellationToken,
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
            as IRabbitConsumerInitializerAsync;
        await instance.Init(rabbitSubscribeDef, cancellationToken);

        // 注册连接断开事件
        await RegisterConnectionShutdownAsync(connection, rabbitSubscribeDef, cancellationToken);
    }

    /// <summary>
    /// 注册连接断开事件，断开后将把所有的订阅重新连接
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="rabbitSubscribeDef"></param>
    protected virtual async Task RegisterConnectionShutdownAsync(
        IConnection connection,
        RabbitSubscribeDef rabbitSubscribeDef,
        CancellationToken cancellationToken
    )
    {
        // 添加连接对应的初始化器字典
        _rabbitConnectionStore.AddOrUpdateQueueSubscribeDef(connection, rabbitSubscribeDef);

        // 判断是否初始化关闭事件，初始化过的话跳过执行
        if (_connectionShutdownDict.ContainsKey(connection))
        {
            return;
        }

        // 加入连接断开字典
        _connectionShutdownDict.TryAdd(connection, rabbitSubscribeDef.ConfigName);

        // 连接断开事件
        connection.ConnectionShutdown += async (sender, ea) =>
        {
            // 移除现有连接关联的数据
            _connectionShutdownDict.TryRemove(connection, out var configName);
            var rabbitConsumerInitializerDict = _rabbitConnectionStore.RemoveAndGetIConnection(
                connection
            );

            // 断开连接重试
            var retryPolicy = _rabbitPolicyStore.GetRetryPolicy(configName);
            await retryPolicy.ExecuteAsync(action: async () =>
            {
                // 创建新连接
                var connectionNew = _rabbitConnectionStore.GetConnection(configName, true);
                // 重新注册原有消费者
                foreach (var item in rabbitConsumerInitializerDict)
                {
                    // 重新订阅
                    await InnerSubscribeAsync(item.Value, cancellationToken, true);
                }
            });
        };
        await Task.Yield();
    }

    #endregion

    #endregion
}
