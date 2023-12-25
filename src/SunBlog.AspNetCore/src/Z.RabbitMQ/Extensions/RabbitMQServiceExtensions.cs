using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Z.RabbitMQ.PublishSubscribe;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.Extensions;

public static class RabbitMQServiceExtensions
{
    /// <summary>
    /// 添加RabbitMq服务注册
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddRabbitMQService(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitConnectionStore, RabbitConnectionStore>();
        services.AddSingleton<IRabbitPolicyStore, RabbitPolicyStore>();
        services.AddSingleton<IRabbitSettingStore>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitSettingStore>>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("RabbitMQ:Connection").Value,
                DispatchConsumersAsync = true
            };

            if (!string.IsNullOrEmpty(configuration.GetSection("RabbitMQ:UserName").Value))
            {
                factory.UserName = configuration.GetSection("RabbitMQ:UserName").Value;
            }

            if (!string.IsNullOrEmpty(configuration.GetSection("RabbitMQ:Password").Value))
            {
                factory.Password = configuration.GetSection("RabbitMQ:Password").Value;
            }

            if (!string.IsNullOrEmpty(configuration.GetSection("RabbitMQ:Port").Value))
            {
                factory.Port = configuration.GetSection("RabbitMQ:Port").Get<int>();
            }

            var retryCount = 5;
            if (!string.IsNullOrEmpty(configuration.GetSection("RabbitMQ:RetryCount").Value))
            {
                retryCount = configuration.GetSection("RabbitMQ:RetryCount").Get<int>();
            }

            return new RabbitSettingStore(factory, logger);
        });
        // 注册序列化服务
        services.AddSingleton<IMsgPackSerializer, MsgPackSerializer>();
        // 注册发布订阅服务
        services.AddTransient<IRabbitSubscriber, RabbitSubscriber>();
        services.AddTransient<IRabbitPublisher, RabbitPublisher>();

        return services;
    }
}
