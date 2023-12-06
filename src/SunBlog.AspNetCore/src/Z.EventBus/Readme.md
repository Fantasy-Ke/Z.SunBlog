﻿﻿﻿## Z.EventBus

基于.NET 平台 C#	语言 提供的Channel打造的异步事件总线库

## [Channel使用](https://learn.microsoft.com/zh-cn/dotnet/core/extensions/channels)

### 使用

- EventDiscriptorAttribute 特性

    ```csharp
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class EventDiscriptorAttribute : Attribute
        {
            /// <summary>
            /// 事件名称
            /// </summary>
            public string EventName { get; private set; }
            /// <summary>
            /// channel 容量设置
            /// </summary>
            public int Capacity { get; private set; }
            /// <summary>
            /// 是否维持一个生产者多个消费者模型
            /// </summary>
            public bool SigleReader { get; private set; }

            public EventDiscriptorAttribute(string eventName, int capacity = 1000, bool sigleReader = true)
            {
                EventName = eventName;
                Capacity = capacity;
                SigleReader = sigleReader;
            }
        }
    ```

- Eto 实现特性

    ```csharp
        [EventDiscriptor("test",1000,false)]
        public class TestEto
        {
            public string Name { get; set; }    

            public string Description { get; set; } 
        }
    ```

- 添加通信管道

    ```csharp
     // 注入事件总线
    builder.Services.AddEventBus();
    ```
- 注入EventBus

    ```csharp
    builder.Services.EventBusSubscribes(c =>
    {
        c.Subscribe<TestDto, TestEventHandler>();
    });
    ```
    
- 创建订阅队列Channle(不使用后台订阅可以不初始化)

    ```csharp
    
    builder.Services.BuildServiceProvider()
        .InitChannles();
    
    ```

- 使用了`Z.Module`模块化可以这样

    - ```c#
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            // 注入事件总线
            context.Services.AddEventBus();
        
            context.Services.EventBusSubscribes(c =>
            {
                c.Subscribe<TestDto, TestEventHandler>();
        
            });
        }
        
        public override void OnInitApplication(InitApplicationContext context)
        {
            context.ServiceProvider.InitChannles();
        }
        ```

#### EventHandler定义

```csharp
public class TestEventHandler : IEventHandler<TestDto>, ITransientDependency
{
    private Microsoft.Extensions.Logging.ILogger _logger;
    public TestEventHandler(ILoggerFactory factory)
    {
        _logger = factory.CreateLogger<TestEventHandler>();
    }
    public Task HandelrAsync(TestDto eto)
    {
        _logger.LogInformation($"{typeof(TestDto).Name}--{eto.Name}--{eto.Description}");
        return Task.CompletedTask;
    }
}
```

#### 添加两个测试方法

- 同步消费事件
    - ```csharp
        /// <summary>
        /// 同步消费事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventCache()
        {
            for (var i = 0; i < 100; i++)
            {
                TestDto eto = new TestDto()
                {
                    Name = "LocalEventBus" + i.ToString(),
                    Description = "zzzz" + i.ToString(),
                };
                await _localEvent.PushAsync(eto);
            }
        
            Log.Warning("我什么时候开始得");
        }
        ```
    - ![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/a4514fa1-939d-4809-a7c4-288e402634c1)

- 队列消费事件
     - ```csharp
        /// <summary>
        /// 后台消费事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventChnnalCache()
        {
            for (var i = 0; i < 100; i++)
            {
                TestDto eto = new TestDto()
                {
                    Name = "LocalEventBus" + i.ToString(),
                    Description = "zzzz" + i.ToString(),
                };
                await _localEvent.EnqueueAsync(eto);
            }
        
            Log.Warning("我什么时候开始得");
        }
        ```
    - ![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/1ac53955-de66-4d9f-83f8-7c2003f0fec6)


