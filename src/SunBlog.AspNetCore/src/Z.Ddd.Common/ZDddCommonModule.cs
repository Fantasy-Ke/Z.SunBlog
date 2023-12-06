using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Z.Ddd.Common.Filters;
using Z.Ddd.Common.Minio;
using Z.Ddd.Common.RedisModule;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;

namespace Z.Ddd.Common
{
    public class ZDddCommonModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            //context.Services.AddAutoMapperSetup();
            var configuration = context.GetConfiguration();

            context.Services.AddControllers(c =>
            {
                c.Filters.Add<ResultFilter>();
            });
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