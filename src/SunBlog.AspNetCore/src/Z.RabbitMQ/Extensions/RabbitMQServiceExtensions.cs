using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Z.RabbitMQ.Manager;
using Z.RabbitMQ.RabbitStore;

namespace Z.RabbitMQ.Extensions;

public static class RabbitMQServiceExtensions
{
    /// <summary>
    /// 添加RabbitMq服务注册
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection ServiceRabbitMQ(
        this IServiceCollection services,
        Action<ConnectionFactory> factoryAction = null
    )
    {
        services.AddSingleton<IRabbitConnectionStore, RabbitConnectionStore>();
        services.AddSingleton<IRabbitPolicyStore, RabbitPolicyStore>();
        services.AddSingleton<IRabbitSettingStore>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitSettingStore>>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("App:RabbitMQ:Connection").Value,
                DispatchConsumersAsync = true
            };

            if (!string.IsNullOrEmpty(configuration.GetSection("App:RabbitMQ:UserName").Value))
            {
                factory.UserName = configuration.GetSection("App:RabbitMQ:UserName").Value;
            }

            if (!string.IsNullOrEmpty(configuration.GetSection("App:RabbitMQ:Password").Value))
            {
                factory.Password = configuration.GetSection("App:RabbitMQ:Password").Value;
            }

            if (!string.IsNullOrEmpty(configuration.GetSection("App:RabbitMQ:Port").Value))
            {
                factory.Port = configuration.GetSection("App:RabbitMQ:Port").Get<int>();
            }

            var retryCount = 5;
            if (!string.IsNullOrEmpty(configuration.GetSection("App:RabbitMQ:RetryCount").Value))
            {
                retryCount = configuration.GetSection("App:RabbitMQ:RetryCount").Get<int>();
            }
            factory.DispatchConsumersAsync = true;
            factoryAction?.Invoke(factory);
            return new RabbitSettingStore(factory, logger);
        });
        // 注册序列化传输服务
        services.AddSingleton<IMsgPackTransmit, MsgPackTransmit>();
        // 注册发布订阅服务
        services.AddTransient<IRabbitEventManager, RabbitEventManager>();

        return services;
    }
}
