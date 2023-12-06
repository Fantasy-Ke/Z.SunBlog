using System.Collections.Concurrent;

namespace Z.EventBus.EventContainer
{
    public interface IEventHandlerContainer
    {
        public ConcurrentBag<EventDiscription> Events { get; }
    }
}
