using RabbitMQ.Client;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Z.RabbitMQ.RabbitStore
{

    /// <summary>
    /// RabbitMQ 配置存储器
    /// </summary>
    public interface IRabbitSettingStore : IDisposable
    {
        /// <summary>
        /// 根据配置名称获取连接工厂
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        IConnectionFactory GetConnectionFactory(string configName = null);


        /// <summary>
        /// 配置连接工厂
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <param name="connectionFactory">工厂实例</param>
        void SetConnectionFactory(IConnectionFactory connectionFactory, string configName = "");

    }


    public class RabbitSettingStore : IRabbitSettingStore
    {

        protected readonly ILogger<IRabbitSettingStore> _logger;


        protected readonly ConcurrentDictionary<string, IConnectionFactory> _dataDict;

        public RabbitSettingStore(IConnectionFactory connectionFactory, ILogger<IRabbitSettingStore> logger = null)
        {
            _logger = logger;
            _dataDict = new ConcurrentDictionary<string, IConnectionFactory>();
            SetConnectionFactory(connectionFactory);
        }

        public virtual IConnectionFactory GetConnectionFactory(string configName = null)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                configName = RabbitConsts.DefaultConfigName;
            }

            _dataDict.TryGetValue(configName, out var connectionFactory);
            return connectionFactory;
        }

        public virtual void SetConnectionFactory(IConnectionFactory connectionFactory, string configName = "")
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                configName = RabbitConsts.DefaultConfigName;
            }

            _dataDict.AddOrUpdate(configName, connectionFactory, (key, OldVal) =>
            {
                return connectionFactory;
            });
        }

        public void Dispose()
        {
            _dataDict.Clear();
        }
    }


}
