using Microsoft.Extensions.DependencyInjection;
using Z.Module;
using Z.Module.Modules;

namespace Z.Fantasy.Core.DynamicProxy;

public class ZCastleCoreModule : ZModule
{
    public override void ConfigureServices(ServiceConfigerContext context)
    {
        context.Services.AddTransient(typeof(ZAsyncDeterminationInterceptor<>));
    }
}
