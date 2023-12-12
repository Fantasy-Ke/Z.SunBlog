using Serilog;
using Z.Fantasy.Core.DynamicProxy;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Aop.AopLog;

public class ZLogAopInterceptor : ZInterceptor, ITransientDependency
{
   // private readonly IServiceScopeFactory _serviceScopeFactory;
    public ZLogAopInterceptor()
    {
       // _serviceScopeFactory = serviceScopeFactory;
    }
    public override async Task InterceptAsync(IZMethodInvocation invocation)
    {
        Log.Warning("日志拦截器,接口开始执行");

        await invocation.ProceedAsync();

        Log.Warning("日志拦截器,接口执行结束");
    }
}
