using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public override Task InterceptAsync(IZMethodInvocation invocation)
    {
        //Log.Error("日志拦截器");
        return Task.CompletedTask;
    }
}
