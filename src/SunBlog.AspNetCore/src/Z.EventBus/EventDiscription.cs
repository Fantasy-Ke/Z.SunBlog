namespace Z.EventBus
{
    public class EventDiscription
    {
        public Type EtoType { get; private set; }

        public Type? HandlerType { get; private set; }

        public EventDiscription(Type etoType, Type? handlerType = null)
        {
            EtoType = etoType;
            HandlerType = handlerType;
        }
    }
}
