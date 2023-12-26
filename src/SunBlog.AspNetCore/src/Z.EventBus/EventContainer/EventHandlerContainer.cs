using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Z.EventBus.Handlers;

namespace Z.EventBus.EventContainer
{
    public class EventHandlerContainer : IEventHandlerContainer
    {
        public ConcurrentBag<EventDiscription> Events { get; private set; }

        private readonly IServiceCollection Services;

        public EventHandlerContainer(IServiceCollection services)
        {
            Events = new ConcurrentBag<EventDiscription>();
            Services = services;
            services.AddSingleton<IEventHandlerContainer>(this);
        }

        private bool Check(Type type)
        {
            var discription = Events.FirstOrDefault(p => p.EtoType == type);

            return discription is null;
        }

        ///订阅并且注入EventHandler
        public void Subscribe(Type eto, Type handler)
        {
            if (!Check(eto))
            {
                return;
            }

            Events.Add(new EventDiscription(eto, handler));

            var handlerbaseType = typeof(IEventHandler<>);

            var handlertype = handlerbaseType.MakeGenericType(eto);

            if (Services.Any(P => P.ServiceType == handlertype))
            {
                return;
            }
            //Services.AddTransient(handler, handlertype);
            Services.AddTransient(handlertype, handler);
        }

        public void Subscribe<TEto, THandler>()
            where TEto : class
            where THandler : IEventHandler<TEto>
        {
            Subscribe(typeof(TEto), typeof(THandler));
        }
    }
}
