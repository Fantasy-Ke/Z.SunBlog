using Microsoft.Extensions.Hosting;

namespace Z.EventBus
{
    public class EventBusBackgroundService : BackgroundService
    {
        private readonly IEventHandlerManager _eventHandlerManager;
        public EventBusBackgroundService(IEventHandlerManager eventHandlerManager)
        {
            _eventHandlerManager = eventHandlerManager;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventHandlerManager.StartConsumer();
        }
    }
}
