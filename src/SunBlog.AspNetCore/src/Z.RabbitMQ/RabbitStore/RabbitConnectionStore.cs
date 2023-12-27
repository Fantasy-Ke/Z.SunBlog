using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

using System.Collections.Concurrent;

namespace Z.RabbitMQ.RabbitStore;

/// <summary>
/// 连接存储器
/// </summary>
public interface IRabbitConnectionStore
{
    /// <summary>
    /// 获取连接
    /// </summary>
    /// <param name="configName">配置名称</param>
    /// <param name="createNew">创建新的，关闭老的</param>
    /// <returns></returns>
    IConnection GetConnection(string configName, bool createNew = false);

    /// <summary>
    /// 获取Channel
    /// </summary>
    /// <param name="connection">连接</param>
    /// <param name="queueName">队列名称</param>
    /// <param name="createNew">创建新的</param>
    /// <returns></returns>
    IModel GetChannel(IConnection connection, string queueName, bool createNew = false);

    /// <summary>
    /// 获取或添加 指定连接中 队列与订阅者的绑定关系
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="rabbitSubscribeDef"></param>
    /// <returns></returns>
    ConcurrentDictionary<string, RabbitSubscribeDef> GetOrAddQueueSubscribeDef
        (IConnection connection, RabbitSubscribeDef rabbitSubscribeDef = null);

    /// <summary>
    /// 删除连接，返回该连接下的所有 队列与订阅者关系字典
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    ConcurrentDictionary<string, RabbitSubscribeDef> RemoveAndGetIConnection(IConnection connection);


    /// <summary>
    /// 修改或添加 指定连接中 队列与订阅者的绑定关系
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    void AddOrUpdateQueueSubscribeDef
        (IConnection connection, RabbitSubscribeDef rabbitSubscribeDef = null);
}


public class RabbitConnectionStore : IRabbitConnectionStore
{
    /// <summary>
    /// key:configName 配置与连接的绑定关系字典 1:1
    /// </summary>
    protected readonly ConcurrentDictionary<string, IConnection> _connectionDict;
    /// <summary>
    /// key:queueName 队列与信道的绑定关系字典 1:1
    /// </summary>
    protected readonly ConcurrentDictionary<string, IModel> _channelDict;
    /// <summary>
    /// key:Iconnection 连接与队列的绑定关系字典 1:n
    /// value:队列与订阅者的绑定关系字典 1:1
    /// </summary>
    protected readonly ConcurrentDictionary<IConnection, ConcurrentDictionary<string, RabbitSubscribeDef>> _connectionRabbitConsumerInitializerDict;


    protected readonly IServiceProvider _serviceProvider;
    protected readonly IRabbitSettingStore _rabbitSettingStore;
    public RabbitConnectionStore(IServiceProvider serviceProvider)
    {
        _connectionDict = new ConcurrentDictionary<string, IConnection>();
        _channelDict = new ConcurrentDictionary<string, IModel>();
        _connectionRabbitConsumerInitializerDict = new ConcurrentDictionary<IConnection, ConcurrentDictionary<string, RabbitSubscribeDef>>();

        _serviceProvider = serviceProvider;
        _rabbitSettingStore = serviceProvider.GetService<IRabbitSettingStore>();
    }
    public IConnection GetConnection(string configName, bool createNew = false)
    {
        if (string.IsNullOrWhiteSpace(configName))
        {
            configName = RabbitConsts.DefaultConfigName;
        }

        var connectionFactory = _rabbitSettingStore.GetConnectionFactory(configName);

        // 创建新的
        if (createNew)
        {
            _connectionDict.TryRemove(configName, out var oldVal);
            oldVal?.Dispose();
        }


        var connection = _connectionDict.GetOrAdd(configName, (key) =>
         {
             return connectionFactory.CreateConnection();
         });
        return connection;
    }

    public IModel GetChannel(IConnection connection, string queueName, bool createNew = false)
    {
        // 创建新的
        if (createNew)
        {
            _channelDict.TryRemove(queueName, out var oldVal);
            oldVal?.Dispose();
        }

        var channel = _channelDict.GetOrAdd(queueName, (key) =>
        {
            return connection.CreateModel();
        });


        // 如果通道已关闭，那么创建新的channel
        if (channel.IsClosed)
        {
            var oldChannel = channel;
            channel = connection.CreateModel();
            _channelDict.TryUpdate(queueName, channel, oldChannel);
        }

        return channel;
    }

    public ConcurrentDictionary<string, RabbitSubscribeDef> GetOrAddQueueSubscribeDef
        (IConnection connection, RabbitSubscribeDef rabbitSubscribeDef = null)
    {
        var rabbitConsumerInitializerDict = _connectionRabbitConsumerInitializerDict
            .GetOrAdd(connection, new ConcurrentDictionary<string, RabbitSubscribeDef>());

        if (rabbitSubscribeDef != null)
        {
            rabbitConsumerInitializerDict.TryAdd(rabbitSubscribeDef.QueueName, rabbitSubscribeDef);
        }

        return rabbitConsumerInitializerDict;
    }

    public void AddOrUpdateQueueSubscribeDef
        (IConnection connection, RabbitSubscribeDef rabbitSubscribeDef = null)
    {
       _connectionRabbitConsumerInitializerDict.TryGetValue(connection,out var rabbitConsumerInitializerDict);

        rabbitConsumerInitializerDict.TryGetValue(rabbitSubscribeDef.QueueName, out var oldRabbitSubscribeDef);
        if (oldRabbitSubscribeDef != null)
        {
            rabbitConsumerInitializerDict.TryUpdate(rabbitSubscribeDef.QueueName,rabbitSubscribeDef, oldRabbitSubscribeDef);
        }
        else
        {
            rabbitConsumerInitializerDict.TryAdd(rabbitSubscribeDef.QueueName, rabbitSubscribeDef);
        }
    }

    public ConcurrentDictionary<string, RabbitSubscribeDef> RemoveAndGetIConnection(IConnection connection)
    {
        _connectionRabbitConsumerInitializerDict.TryRemove(connection, out var rabbitConsumerInitializerDict);

        return rabbitConsumerInitializerDict;
    }
}
