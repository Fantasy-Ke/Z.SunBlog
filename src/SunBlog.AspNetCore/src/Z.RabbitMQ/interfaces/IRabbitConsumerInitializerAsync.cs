using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.RabbitMQ.interfaces;

/// <summary>
/// 消费者初始化接口
/// </summary>
public interface IRabbitConsumerInitializerAsync
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="channel"></param>
    Task Init(RabbitSubscribeDef rabbitSubscribeDef, CancellationToken cancellationToken = default);
}
