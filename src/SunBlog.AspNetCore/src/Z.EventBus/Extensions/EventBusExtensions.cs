using Microsoft.Extensions.DependencyInjection;
using Z.EventBus.EventBus;
using Z.EventBus.EventContainer;
using Z.EventBus.Manager;

namespace Z.EventBus.Extensions
{
    public static class EventBusExtensions
    {
        //添加事件总线并且添加channle管道
        public static IServiceCollection AddEventBusAndSubscribes(this IServiceCollection services, Action<EventHandlerContainer> action)
        {
            services.AddSingleton<IEventHandlerManager, EventHandlerManager>();

            services.AddTransient<ILocalEventBus, LocalEventBus>();

            services.AddHostedService<EventBusBackgroundService>();

            EventHandlerContainer eventHandlerContainer = new EventHandlerContainer(services);

            action.Invoke(eventHandlerContainer);

            return services;
        }

        //创建通信管道
        public static async Task InitChannles(this IServiceProvider serviceProvider, Action<IEventHandlerManager> action)
        {
            var scope = serviceProvider.CreateAsyncScope();

            var eventhandlerManager = scope.ServiceProvider.GetRequiredService<IEventHandlerManager>();

            await eventhandlerManager.CreateChannles();

            action.Invoke(eventhandlerManager);
        }

        //添加本地事件总线
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventHandlerManager, EventHandlerManager>();

            services.AddTransient<ILocalEventBus, LocalEventBus>();

            services.AddHostedService<EventBusBackgroundService>();

            return services;
        }

        //添加通信管道
        public static IServiceCollection Subscribes(this IServiceCollection services, Action<EventHandlerContainer> action)
        {
            EventHandlerContainer eventHandlerContainer = new EventHandlerContainer(services);

            action.Invoke(eventHandlerContainer);

            return services;
        }
    }
}
