using System.Collections.Concurrent;

namespace Z.EventBus
{
    public interface IEventHandlerContainer
    {
        public ConcurrentBag<EventDiscription> Events { get; }
    }
}
