using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.PublishSubscribe;

/// <summary>
/// 订阅者
/// </summary>
public interface IRabbitSubscriber : IDisposable
{
    /// <summary>
    /// 订阅
    /// </summary>
    /// <typeparam name="T">消费者类型类型</typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">rabbitmq队列名称</param>
    /// <param name="queueCount">负载均衡数/队列数</param>
    /// <param name="xMaxPriority">消息最大优先级</param>
    void Subscribe<T>(string configName, string queueName, int queueCount = 1,
        int xMaxPriority = 0, bool isDLX = false)
        where T : IRabbitConsumerInitializer;

    /// <summary>
    /// 订阅死信队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">队列名称</param>
    void SubscribeDLX<T>(string configName, string queueName)
        where T : IRabbitConsumerInitializer;

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <typeparam name="T">消费者类型类型</typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">rabbitmq队列名称</param>
    void UnSubscribe<T>(string configName, string queueName)
        where T : IRabbitConsumerInitializer;
}

public class RabbitSubscriber : IRabbitSubscriber
{
    protected readonly ConcurrentDictionary<IConnection, string> _connectionShutdownDict;
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IRabbitConnectionStore _rabbitConnectionStore;
    protected readonly IRabbitPolicyStore _rabbitPolicyStore;

    public RabbitSubscriber(IServiceProvider serviceProvider, IRabbitConnectionStore rabbitConnectionStore, IRabbitPolicyStore rabbitPolicyStore)
    {
        _serviceProvider = serviceProvider;
        _rabbitConnectionStore = rabbitConnectionStore;
        _rabbitPolicyStore = rabbitPolicyStore;

        _connectionShutdownDict = new ConcurrentDictionary<IConnection, string>();
    }

    /// <inheritdoc/>
    public void Subscribe<T>(string configName, string queueName, int queueCount = 1,
        int xMaxPriority = 0, bool isDLX = false)
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
        retryPolicy.ExecuteAsync(async () =>
        {
            if (subscribeDef.IsLoadBalancing)
            {
                subscribeDef.InitLoadBalancing();
            }

            // 订阅
            InnerSubscribe(subscribeDef);

        }).GetAwaiter();

    }
    /// <inheritdoc/>
    public void SubscribeDLX<T>(string configName, string queueName)
        where T : IRabbitConsumerInitializer
    {
        var dlxQueueName = RabbitConsts.DLXQueuePrefix + queueName;
        Subscribe<T>(configName, dlxQueueName, isDLX: true);
    }
    /// <inheritdoc/>
    public void UnSubscribe<T>(string configName, string queueName)
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
    protected virtual void InnerSubscribe(RabbitSubscribeDef rabbitSubscribeDef, bool createNewChannel = false)
    {
        // 获取连接，这里永远都是最新的
        var connection = _rabbitConnectionStore.GetConnection(rabbitSubscribeDef.ConfigName);

        // 获取订阅通道
        var channel = _rabbitConnectionStore.GetChannel(connection, rabbitSubscribeDef.QueueName, createNewChannel);
        rabbitSubscribeDef.Channel = channel;

        // 获取消费者实例,并初始化
        var instance = _serviceProvider.GetRequiredService(rabbitSubscribeDef.RabbitConsumerInitializerType) as IRabbitConsumerInitializer;
        instance.Init(rabbitSubscribeDef);

        // 注册连接断开事件
        RegisterConnectionShutdown(connection, rabbitSubscribeDef);
    }

    /// <summary>
    /// 注册连接断开事件，断开后将把所有的订阅重新连接
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="rabbitSubscribeDef"></param>
    protected virtual void RegisterConnectionShutdown(IConnection connection, RabbitSubscribeDef rabbitSubscribeDef)
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
            var rabbitConsumerInitializerDict = _rabbitConnectionStore.RemoveAndGetIConnection(connection);

            // 断开连接重试
            var retryPolicy = _rabbitPolicyStore.GetRetryPolicy(configName);
            retryPolicy.ExecuteAsync(async () =>
            {
                // 创建新连接
                var connectionNew = _rabbitConnectionStore.GetConnection(configName, true);
                // 重新注册原有消费者
                foreach (var item in rabbitConsumerInitializerDict)
                {
                    // 重新订阅
                    InnerSubscribe(item.Value, true);
                }
            }).GetAwaiter();
        };
    }

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
