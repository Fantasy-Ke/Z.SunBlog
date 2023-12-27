using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.RabbitMQ.interfaces;

/// <summary>
/// 消费者基础接口
/// </summary>
/// <typeparam name="T">处理类型</typeparam>
public interface IRabbitConsumer<T> : IRabbitConsumerInitializer
{
    void Exec(T eventArgs);
}
