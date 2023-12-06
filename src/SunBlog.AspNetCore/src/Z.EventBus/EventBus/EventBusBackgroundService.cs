using Microsoft.Extensions.Hosting;
using Z.EventBus.Manager;

namespace Z.EventBus.EventBus
{
    public class EventBusBackgroundService : BackgroundService
    {
        private readonly IEventHandlerManager _eventHandlerManager;
        public EventBusBackgroundService(IEventHandlerManager eventHandlerManager)
        {
            _eventHandlerManager = eventHandlerManager;
        }

        /// <summary>
        /// 执行队列中得任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventHandlerManager.StartConsumer();
        }
    }
}
