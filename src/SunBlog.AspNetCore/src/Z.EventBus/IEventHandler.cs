namespace Z.EventBus
{
    public interface IEventHandler<TEto> where TEto : class
    {
        Task HandelrAsync(TEto eto);
    }
}
