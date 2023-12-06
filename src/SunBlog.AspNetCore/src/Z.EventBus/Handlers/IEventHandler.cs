namespace Z.EventBus.Handlers
{
    public interface IEventHandler<TEto> where TEto : class
    {
       Task HandelrAsync(TEto eto);
    }
}
