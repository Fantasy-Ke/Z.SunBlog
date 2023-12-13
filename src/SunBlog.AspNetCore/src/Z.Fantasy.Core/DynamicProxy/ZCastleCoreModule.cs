using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Module;
using Z.Module.Modules;

namespace Z.Fantasy.Core.DynamicProxy;

public class ZCastleCoreModule:ZModule
{
    public override void ConfigureServices(ServiceConfigerContext context)
    {
        context.Services.AddAsyncDeterminationTransient();
        //context.Services.DisableZClassInterceptors();禁用类拦截Aop
    }
}
