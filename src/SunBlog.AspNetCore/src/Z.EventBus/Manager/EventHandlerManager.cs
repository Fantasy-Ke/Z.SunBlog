using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using Z.EventBus.Attributes;
using Z.EventBus.EventContainer;
using Z.EventBus.Exceptions;
using Z.EventBus.Handlers;

namespace Z.EventBus.Manager
{
    public class EventHandlerManager: IEventHandlerManager, IDisposable 
    {
        private ConcurrentDictionary<string, Channel<string>> Channels;

        private bool IsDiposed = false;

        private readonly IServiceProvider ServiceProvider;

        private readonly CancellationToken _cancellation;

        private readonly IEventHandlerContainer _eventHandlerContainer;

        private readonly ILogger _logger;

        private bool IsInitConsumer = true;

        public EventHandlerManager(
            IServiceProvider serviceProvider,
            IEventHandlerContainer eventHandlerContainer,
            ILoggerFactory loggerFactory
        )
        {
            ServiceProvider = serviceProvider;
            _cancellation = CancellationToken.None;
            _eventHandlerContainer = eventHandlerContainer;
            Channels = new ConcurrentDictionary<string, Channel<string>>();
            _logger = loggerFactory.CreateLogger<IEventHandlerManager>();
        }

        public async Task CreateChannles()
        {
            var eventDiscriptions = _eventHandlerContainer.Events;

            foreach (var item in eventDiscriptions)
            {
                var attribute = item.EtoType
                    .GetCustomAttributes()
                    .OfType<EventDiscriptorAttribute>()
                    .FirstOrDefault();

                if (attribute is null)
                {
                    ThorwEventAttributeNullException.ThorwException();
                }

                var channel = Channels.GetValueOrDefault(attribute.EventName);

                if (channel is not null)
                {
                    return;
                }

                channel = Channel.CreateBounded<string>(
                    new BoundedChannelOptions(attribute.Capacity)
                    {
                        SingleWriter = true,
                        SingleReader = attribute.SigleReader,
                        AllowSynchronousContinuations = false, // 异步方式执行
                        FullMode = BoundedChannelFullMode.Wait // 等待存在空间进行写入
                    }
                );

                Channels.TryAdd(attribute.EventName, channel);

                _logger.LogInformation($"创建通信管道{item.EtoType}--{attribute.EventName}");
            }
            await Task.CompletedTask;
        }

        private Channel<string> Check(Type type)
        {
            var attribute = type.GetCustomAttributes()
                .OfType<EventDiscriptorAttribute>()
                .FirstOrDefault();

            if (attribute is null)
            {
                ThorwEventAttributeNullException.ThorwException();
            }

            var channel = Channels.GetValueOrDefault(attribute.EventName);

            if (channel is null)
            {
                ThrowChannelNullException.ThrowException(attribute.EventName);
            }

            return channel;
        }

        public void Dispose()
        {
            IsDiposed = true;
            IsInitConsumer = true;
            _cancellation.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <typeparam name="TEto"></typeparam>
        /// <param name="eto"></param>
        /// <returns></returns> 
        public async Task EnqueueAsync<TEto>(TEto eto)
            where TEto : class
        {
            var channel = Check(typeof(TEto));

            while (await channel.Writer.WaitToWriteAsync())
            {
                var data = JsonConvert.SerializeObject(eto);

                await channel.Writer.WriteAsync(data, _cancellation);

                break;
            }
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <typeparam name="TEto"></typeparam>
        /// <param name="eto"></param>
        /// <returns></returns>
        public async Task ExecuteAsync<TEto>(TEto eto)
            where TEto : class
        {
            Check(typeof(TEto));

            var scope = ServiceProvider.CreateAsyncScope();

            var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEto>>();

            await handler.HandelrAsync(eto);
        }

        /// <summary>
        /// 后台消费者
        /// </summary>
        /// <returns></returns>
        public async Task StartConsumer()
        {
            if (!IsInitConsumer)
            {
                return;
            }

            foreach (var item in _eventHandlerContainer.Events)
            {
                _ = Task.Factory.StartNew(async () =>
                {


                    var channel = Check(item.EtoType);

                    var handlerType = typeof(IEventHandler<>).MakeGenericType(item.EtoType);
                    var scope = ServiceProvider.CreateAsyncScope();
                    var handler = scope.ServiceProvider.GetRequiredService(handlerType);

                    var reader = channel.Reader;

                    try
                    {
                        while (await channel.Reader.WaitToReadAsync())
                        {
                            while (reader.TryRead(out string str))
                            {
                                var data = JsonConvert.DeserializeObject(str, item.EtoType);

                                _logger.LogInformation(str);

                                await (Task)
                                    handlerType
                                        .GetMethod("HandelrAsync")
                                        .Invoke(handler, new object[] { data });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation($"本地事件总线异常{e.Source}--{e.Message}--{e.Data}");
                        throw;
                    }
                });
            }
            IsInitConsumer = false;
            await Task.CompletedTask;
        }
    }
}
