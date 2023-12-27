using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Z.RabbitMQ.interfaces;
namespace Z.RabbitMQ;

/// <summary>
/// 消费者基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RabbitConsumer<T> : IRabbitConsumer<T>
{
    private readonly IMsgPackTransmit _serializer;
    private readonly ILogger<RabbitConsumer<T>> _logger;

    protected RabbitConsumer(IServiceProvider serviceProvider, ILogger<RabbitConsumer<T>> logger)
    {
        _serializer = serviceProvider.GetService<IMsgPackTransmit>();
        _logger = logger;
    }

    /// <inheritdoc/>
    public virtual void Init(RabbitSubscribeDef rabbitSubscribeDef)
    {
        var channel = rabbitSubscribeDef.Channel;
        // 源队列名称（非负载均衡前的）
        var sourceQueueName = rabbitSubscribeDef.QueueName;
        // 队列数大于1，则开启负载均衡订阅
        if (rabbitSubscribeDef.IsLoadBalancing)
        {
            foreach (var queueName in rabbitSubscribeDef.LoadBalancing.GetAll())
            {
                CreateQueue(sourceQueueName, queueName, channel, rabbitSubscribeDef.XMaxPriority);
            }
        }
        else
        {
            if (rabbitSubscribeDef.IsDLXQueue)
            {
                CreateDLXQueue(sourceQueueName, channel);
                return;
            }
            CreateQueue(sourceQueueName, sourceQueueName, channel, rabbitSubscribeDef.XMaxPriority);
        }
    }

    /// <summary>
    /// 消费者处理消息的内部逻辑处理
    /// </summary>
    /// <param name="eventArgs"></param>
    public abstract void Exec(T eventArgs);

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
        channel.ExchangeDeclare(queueName, ExchangeType.Fanout, false, false, null);
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

        _logger.LogInformation($"【订阅信息】{queueName}");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                _logger.LogInformation(
                    exchangeName + "处理了该消息，消息优先级为：" + ea.BasicProperties.Priority
                );
                // 数据反序列化为T
                var body = ea.Body.ToArray();
                var eventArgs = _serializer.BytesToMessage<T>(body);

                // TODO: 实际处理逻辑
                Exec(eventArgs);

                // 手动消费，确认消息处理完成
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError("【异常消息】：" + ex.Message);

                // 创建死信交换机
                channel.ExchangeDeclare(
                    dlxexChange,
                    type: ExchangeType.Direct,
                    durable: true,
                    autoDelete: false
                );
                // 创建死信队列
                channel.QueueDeclare(
                    dlxQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false
                );
                // 死信队列绑定死信交换机
                channel.QueueBind(dlxQueueName, dlxexChange, routingKey: dlxQueueName);

                // 拒绝消息，将消息排入死信队列
                channel.BasicReject(deliveryTag: ea.DeliveryTag, false);
                _logger.LogError($"【存入死信队列】：{dlxQueueName},【死信交换机】{dlxexChange}");
            }
            finally
            {
                await Task.Yield();
            }
        };

        // 启动消费者 消费者和channel绑定，并指定要处理哪个队列 关闭自动确认
        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    /// <summary>
    /// 创建死信队列
    /// </summary>
    /// <param name="dlxQueueName"></param>
    /// <param name="channel"></param>
    private void CreateDLXQueue(string dlxQueueName, IModel channel)
    {
        // 死信交换机名称
        var dlxexChange = dlxQueueName;
        // 创建死信交换机
        channel.ExchangeDeclare(
            dlxexChange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );
        // 定义死信队列
        channel.QueueDeclare(
            queue: dlxQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        channel.QueueBind(dlxQueueName, dlxexChange, routingKey: dlxQueueName);

        // 设置公平调度
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation($"【订阅信息】{dlxQueueName}");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                _logger.LogInformation(
                    dlxexChange + "处理了该消息，消息优先级为：" + ea.BasicProperties.Priority
                );
                // 数据反序列化为T
                var body = ea.Body.ToArray();
                var eventArgs = _serializer.BytesToMessage<T>(body);

                // 实际处理逻辑
                Exec(eventArgs);
            }
            catch (Exception ex)
            {
                _logger.LogError("【死信处理】异常消息：" + ex.Message);
            }
            finally
            {
                // 手动消费，确认消息处理完成
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                await Task.Yield();
            }
        };

        channel.BasicConsume(queue: dlxQueueName, autoAck: false, consumer: consumer);
    }
}
