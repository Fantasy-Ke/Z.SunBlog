﻿﻿﻿## Z.RabbitMQ

基于.NET 平台 C#	语言 提供的RabbitM负载均衡

## [RabbitMQ使用](https://www.rabbitmq.com/)

### 使用
- 添加通信管道

    ```csharp
     // 注入事件总线
    services.ServiceRabbitMQ();
    ```
- 存在参数`Action<ConnectionFactory>`配置默认的链接Factory

    ```csharp
    services.ServiceRabbitMQ(c =>
    {
        c.DispatchConsumersAsync = false; //关闭异步
    });
    ```

#### IRabbitEventManager定义

- 构造函数注入`IRabbitEventManager`

```csharp
 private readonly IRabbitEventManager _rabbitEventManager;
```

#### 消费者抽象类

- 继承`RabbitConsumer<T>`
  - 实现`void Exec`接口
- 继承`RabbitConsumerAsync<T>`
  - 实现`Task Exec`接口

#### 测试方法

- `CommentsConsumer`是继承抽象类消费者
- `Comments`是传输数据的Data
- `"comment"`队列名称

- 推送rabbit队列
    
    - ```csharp
        /// <summary>
        /// 推送rabbit队列
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task RabbitPublish()
        {
            await _rabbitEventManager.PublishAsync<CommentsConsumer, Comments>(
                 "comment",
                 new Comments()
                 {
                     //"moduleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                     // "rootId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                     // "parentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                     // "replyAccountId": "string",
                     // "content": "fdsafd发顺丰"
                     ModuleId = Guid.NewGuid(),
                     RootId = Guid.NewGuid(),
                     ParentId = Guid.NewGuid(),
                     ReplyAccountId = Guid.NewGuid().ToString("N"),
                     Content = $"测试消息队列 Guid：{Guid.NewGuid()}"
                 }
             );
        }
        ```
    
- 订阅rabbit队列

     - ```csharp
         /// <summary>
         /// 订阅rabbit队列
         /// </summary>
         /// <returns></returns>
         [HttpGet]
         public async Task RabbitSubscribe()
         {
             await _rabbitEventManager.SubscribeAsync<CommentsConsumer>("comment");
         }
        ```

     - ```c#
       public async Task SubscribeAsync<T>(
           string queueName,//队列名称
           string configName = "",//链接配置名称
           int queueCount = 1,//队列个数
           int xMaxPriority = 0,//优先级
           bool isDLX = false,//是否死信
           CancellationToken cancellationToken = default
       )
       ```

       

- 消费死信队列

  - ```C#
    /// <summary>
    /// 消费死信队列
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task RabbitSubscribeDLXAsync()
    {
        await _rabbitEventManager.SubscribeDLXAsync<CommentsConsumer>("comment");
    }
    ```

- 取消订阅

  - ```C#
     /// <summary>
     /// 取消订阅
     /// </summary>
     /// <returns></returns>
     [HttpGet]
     public async Task RabbitUnSubscribe()
     {
         await _rabbitEventManager.UnSubscribeAsync<CommentsConsumer>("comment");
     }
    ```

    

