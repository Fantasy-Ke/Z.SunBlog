namespace Z.EventBus
{
    public class LocalEventBus : ILocalEventBus
    {
        private readonly IEventHandlerManager _eventHandlerManager;
        public LocalEventBus(IEventHandlerManager eventHandlerManager)
        {
            _eventHandlerManager = eventHandlerManager;
        }
        public async Task EnqueueAsync<TEto>(TEto eto) where TEto : class
        {
            await _eventHandlerManager.EnqueueAsync(eto);
        }

        public async Task PublichAsync<TEto>(TEto eto) where TEto : class
        {
            await _eventHandlerManager.ExecuteAsync(eto);
        }
    }
}
