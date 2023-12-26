using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.RabbitMQ.Manager;

public interface IRabbitEventManager : IDisposable
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
        (string queueName, TEventData eventData, string configName = "", int priority = 0, int queueCount = 1, int xMaxPriority = 0)
        where TRabbitConsumer : IRabbitConsumer<TEventData>, IRabbitConsumerInitializer;


    /// <summary>
    /// 订阅
    /// </summary>
    /// <typeparam name="T">消费者类型类型</typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">rabbitmq队列名称</param>
    /// <param name="queueCount">负载均衡数/队列数</param>
    /// <param name="xMaxPriority">消息最大优先级</param>
    void Subscribe<T>(string queueName, string configName = "", int queueCount = 1,
        int xMaxPriority = 0, bool isDLX = false)
        where T : IRabbitConsumerInitializer;

    /// <summary>
    /// 订阅死信队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">队列名称</param>
    void SubscribeDLX<T>(string queueName, string configName = "")
        where T : IRabbitConsumerInitializer;

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <typeparam name="T">消费者类型类型</typeparam>
    /// <param name="configName">rabbitmq连接配置名称</param>
    /// <param name="queueName">rabbitmq队列名称</param>
    void UnSubscribe<T>(string queueName, string configName = "")
        where T : IRabbitConsumerInitializer;
}
