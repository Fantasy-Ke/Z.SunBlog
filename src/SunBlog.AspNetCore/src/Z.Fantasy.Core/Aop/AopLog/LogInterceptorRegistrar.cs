using System;
using System.Linq;
using Z.Fantasy.Core.DependencyInjection;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.Helper;

namespace Z.Fantasy.Core.Aop.AopLog;

public static class LogInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<ZLogAopInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        if (AppSettings.GetValue("App:LogAOP:Enabled").CastTo(true))
        {
            return true;
        }

        return false;
    }
}
