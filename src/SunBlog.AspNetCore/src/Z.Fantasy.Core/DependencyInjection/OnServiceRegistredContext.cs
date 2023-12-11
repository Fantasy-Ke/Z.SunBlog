using System;
using JetBrains.Annotations;
using Z.Fantasy.Core.DynamicProxy;
using Z.Fantasy.Core.Exceptions;
using Z.Module.Collections;

namespace Z.Fantasy.Core.DependencyInjection;

public class OnServiceRegistredContext : IOnServiceRegistredContext
{
    public virtual ITypeList<IZInterceptor> Interceptors { get; }

    public virtual Type ServiceType { get; }

    public virtual Type ImplementationType { get; }

    public OnServiceRegistredContext(Type serviceType, Type implementationType)
    {
        ServiceType = nameof(serviceType) != null ? serviceType : throw new UserFriendlyException("serviceType¿Õ");
        ImplementationType = nameof(implementationType) != null ? implementationType : throw new UserFriendlyException("implementationType¿Õ");
        Interceptors = new TypeList<IZInterceptor>();
    }
}
