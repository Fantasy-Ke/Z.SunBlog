using Microsoft.Extensions.DependencyInjection;
using Z.EventBus.EventBus;
using Z.EventBus.EventContainer;
using Z.EventBus.Manager;

namespace Z.EventBus.Extensions
{
    public static class EventBusExtensions
    {
        /// <summary>
        /// 添加事件总线并且添加<code>EventHandlerContainer</code>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBusAndSubscribes(this IServiceCollection services, Action<EventHandlerContainer> action)
        {
            services.AddSingleton<IEventHandlerManager, EventHandlerManager>();

            services.AddTransient<ILocalEventBus, LocalEventBus>();

            services.AddHostedService<EventBusBackgroundService>();

            EventHandlerContainer eventHandlerContainer = new EventHandlerContainer(services);

            action.Invoke(eventHandlerContainer);

            return services;
        }

        /// <summary>
        /// 创建通信管道
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task InitChannlesAsync(this IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateAsyncScope();

            var eventhandlerManager = scope.ServiceProvider.GetRequiredService<IEventHandlerManager>();

            await eventhandlerManager.CreateChannles();
        }

        /// <summary>
        /// 创建通信管道
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void InitChannles(this IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();

            var eventhandlerManager = scope.ServiceProvider.GetRequiredService<IEventHandlerManager>();

            eventhandlerManager.CreateChannles().Wait();
        }

        /// <summary>
        /// 添加本地事件总线
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventHandlerManager, EventHandlerManager>();

            services.AddTransient<ILocalEventBus, LocalEventBus>();

            services.AddHostedService<EventBusBackgroundService>();

            return services;
        }

        /// <summary>
        /// 添加通信管道
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection EventBusSubscribes(this IServiceCollection services, Action<EventHandlerContainer> action)
        {
            EventHandlerContainer eventHandlerContainer = new EventHandlerContainer(services);

            action.Invoke(eventHandlerContainer);

            return services;
        }
    }
}
