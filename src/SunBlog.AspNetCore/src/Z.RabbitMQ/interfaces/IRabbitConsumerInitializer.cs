using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.RabbitMQ.interfaces;

/// <summary>
/// 消费者初始化接口
/// </summary>
public interface IRabbitConsumerInitializer
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="channel"></param>
    void Init(RabbitSubscribeDef rabbitSubscribeDef);
}
