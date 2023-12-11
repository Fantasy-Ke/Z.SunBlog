using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.Aop.AopLog;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.DynamicProxy;
using Z.Fantasy.Core.Filters;
using Z.Fantasy.Core.Minio;
using Z.Fantasy.Core.RedisModule;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;

namespace Z.Fantasy.Core
{
    [DependOn(typeof(ZCastleCoreModule))]
    public class ZFantasyCoreModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            //context.Services.AddAutoMapperSetup();
            var configuration = context.GetConfiguration();

            context.Services.AddControllers(c =>
            {
                c.Filters.Add<ResultFilter>();
            });

            context.Services.OnRegistered(LogInterceptorRegistrar.RegisterIfNeeded);
            //context.UseAutofac();

            //redis注册
            context.Services.AddRedis(configuration);

            context.Services.AddMinio(configuration);

            context.Services.AddTransient<IMinioService, MinioService>();
        }

        public override void PostInitApplication(InitApplicationContext context)
        {
            var scope = context.ServiceProvider.CreateAsyncScope();
            //minio需要配置https
            scope.ServiceProvider
               .GetRequiredService<IMinioService>()
               .CreateDefaultBucket();

        }
    }
}