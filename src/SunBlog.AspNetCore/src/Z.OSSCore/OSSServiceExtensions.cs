using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Minio;
using System.Xml.Linq;
using Z.OSSCore.EntityType;
using Z.OSSCore.Interface;
using Z.OSSCore.Models;
using Z.OSSCore.Providers;

namespace Z.OSSCore
{
    public static class OSSServiceExtensions
    {
        /// <summary>
        /// �������ļ��м���
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IServiceCollection AddOSSService(this IServiceCollection services, string key = "App:SSOConfig", Action<OSSOptions> oSSOptions = null)
        {
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                if (configuration == null)
                {
                    throw new ArgumentNullException(nameof(IConfiguration));
                }
                IConfigurationSection section = configuration.GetSection(key);
                OSSOptions options = section.Get<OSSOptions>() ?? new OSSOptions();

                oSSOptions?.Invoke(options);

                if (!options.Enable)
                    return services;
                services.Configure<OSSOptions>(option => { 
                    option.Provider = options.Provider;
                    option.Enable = options.Enable;
                    option.DefaultBucket = options.DefaultBucket;
                    option.Endpoint = options.Endpoint;  //����Ҫ����Э��
                    option.AccessKey = options.AccessKey;
                    option.SecretKey = options.SecretKey;
                    option.IsEnableHttps = options.IsEnableHttps;
                    option.IsEnableCache = options.IsEnableCache; 
                });
                //����IOSSServiceFactoryֻ��Ҫע��һ��
                if (services.All(p => p.ServiceType != typeof(IOSSServiceFactory<>)))
                {
                    //���δע��ICacheProvider��Ĭ��ע��MemoryCacheProvider
                    if (services.All(p => p.ServiceType != typeof(ICacheProvider)))
                    {
                        services.AddMemoryCache();
                        services.TryAddSingleton<ICacheProvider, MemoryCacheProvider>();
                    }
                    services.AddSingleton(typeof(IOSSServiceFactory<>),typeof(OSSServiceFactory<>));
                }
                //
                switch (options.Provider)
                {
                    case OSSProvider.Aliyun:
                        services.TryAddSingleton(sp => sp.GetRequiredService<IOSSServiceFactory<OSSAliyun>>().Create());
                        break;
                    case OSSProvider.Minio:
                        services.TryAddSingleton(sp => sp.GetRequiredService<IOSSServiceFactory<OSSMinio>>().Create());
                        break;
                }

                return services;
            }
        }

        /// <summary>
        /// ����Ĭ������
        /// </summary>
        public static IServiceCollection AddOSSService(this IServiceCollection services, Action<OSSOptions> option)
        {
            return services.AddOSSService(oSSOptions: option);
        }
    }
}