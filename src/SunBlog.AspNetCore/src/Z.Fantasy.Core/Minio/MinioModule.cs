using Microsoft.Extensions.DependencyInjection;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;

namespace Z.Fantasy.Core.Minio;

public class MinioModule// : ZModule
{
    //public override void ConfigureServices(ServiceConfigerContext context)
    //{
    //    var configurarion = context.GetConfiguration();

    //    context.Services.AddMinio(configurarion);

    //    context.Services.AddTransient<IMinioService, MinioService>();
    //}

    //public override void PostInitApplication(InitApplicationContext context)
    //{
    //    var scope = context.ServiceProvider.CreateAsyncScope();

    //    //minio需要配置https
    //    scope.ServiceProvider
    //       .GetRequiredService<IMinioService>()
    //       .CreateDefaultBucket();

    //}
}
