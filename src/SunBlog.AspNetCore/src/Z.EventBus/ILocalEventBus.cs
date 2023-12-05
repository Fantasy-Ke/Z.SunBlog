namespace Z.EventBus
{
    public interface ILocalEventBus
    {
        Task PublichAsync<TEto>(TEto eto) where TEto : class;

        Task EnqueueAsync<TEto>(TEto eto) where TEto : class;
    }
}
