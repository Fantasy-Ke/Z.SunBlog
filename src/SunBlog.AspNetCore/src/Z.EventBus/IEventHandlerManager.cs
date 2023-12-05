namespace Z.EventBus
{
    public interface IEventHandlerManager
    {
        Task EnqueueAsync<TEto>(TEto eto) where TEto : class;

        Task CreateChannles();
        Task StartConsumer();

        Task ExecuteAsync<TEto>(TEto eto) where TEto : class;
    }
}
