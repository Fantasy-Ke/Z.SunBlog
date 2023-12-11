using System;
using Z.Fantasy.Core.DynamicProxy;
using Z.Module.Collections;

namespace Z.Fantasy.Core.DependencyInjection;

public interface IOnServiceRegistredContext
{
    ITypeList<IZInterceptor> Interceptors { get; }

    Type ImplementationType { get; }
}
