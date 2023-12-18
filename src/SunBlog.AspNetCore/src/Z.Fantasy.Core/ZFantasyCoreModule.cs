using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.Aop.AopLog;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.DynamicProxy;
using Z.Fantasy.Core.Filters;
using Z.Fantasy.Core.Minio;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Builder;
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
            //context.Services.RegisterJobs();
            context.Services.AddControllers(c =>
            {
                c.Filters.Add<ResultFilter>();
            });

            //context.Services.AddAsyncDeterminationTransient();
            //context.Services.DisableZClassInterceptors();禁用类拦截Aop
            //Registered拦截器
            context.Services.OnRegistered(LogInterceptorRegistrar.RegisterIfNeeded);
            //context.UseAutofac();
            var configuration = context.GetConfiguration();
            
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