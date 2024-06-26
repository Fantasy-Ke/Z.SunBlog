﻿using Autofac.Builder;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Z.Module.Modules.interfaces;
using Z.Module.Extensions;
using System.Reflection;
using Z.Foundation.Core.DependencyInjection.Extensions;
using Z.Foundation.Core.AutofacExtensions.Builder;

namespace Z.Foundation.Core.AutofacExtensions.Extensions;

public static class AutofacRegistration
{
    /// <summary>
    /// Populates the Autofac container builder with the set of registered service descriptors
    /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
    /// available in the container.
    /// </summary>
    public static void Populate(
        this ContainerBuilder builder,
        IServiceCollection services)
    {
        builder.Populate(services, null);
    }

    /// <summary>
    /// 抄着abp vnext的
    /// Populates the Autofac container builder with the set of registered service descriptors
    /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
    /// available in the container. Using this overload is incompatible with the ASP.NET Core
    /// support for <see cref="IServiceProviderFactory{TContainerBuilder}"/>.
    /// </summary>
    public static void Populate(
        this ContainerBuilder builder,
        IServiceCollection services,
        object lifetimeScopeTag)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        builder.RegisterType<AutofacServiceProvider>()
            .As<IServiceProvider>()
            .As<IServiceProviderIsService>()
            .ExternallyOwned();

        var autofacServiceScopeFactory = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory");
        if (autofacServiceScopeFactory == null)
        {
            throw new Exception("Unable get type of Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory!");
        }

        // Issue #83: IServiceScopeFactory must be a singleton and scopes must be flat, not hierarchical.
        builder
            .RegisterType(autofacServiceScopeFactory)
            .As<IServiceScopeFactory>()
            .SingleInstance();

        Register(builder, services);
    }

    /// <summary>
    /// 注入生命周期
    /// </summary>
    private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceLifetime lifecycleKind)
    {
        switch (lifecycleKind)
        {
            case ServiceLifetime.Singleton:
                registrationBuilder.SingleInstance();
                break;
            case ServiceLifetime.Scoped:
                registrationBuilder.InstancePerLifetimeScope();
                break;
            case ServiceLifetime.Transient:
                registrationBuilder.InstancePerDependency();
                break;
        }

        return registrationBuilder;
    }

    /// <summary>
    /// Populates the Autofac container builder with the set of registered service descriptors.
    /// </summary>
    private static void Register(ContainerBuilder builder, IServiceCollection services)
    {
        var moduleContainer = services.GetSingletonInstance<IModuleContainer>();
        var registrationActionList = services.GetRegistrationActionList();
        foreach (var descriptor in services)
        {
            if (descriptor.ImplementationType != null)
            {
                var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                if (serviceTypeInfo.IsGenericTypeDefinition)//泛型Register
                {
                    builder
                        .RegisterGeneric(descriptor.ImplementationType)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .ConfigureZConventions(moduleContainer, registrationActionList);
                }
                else
                {
                    builder
                        .RegisterType(descriptor.ImplementationType)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .ConfigureZConventions(moduleContainer, registrationActionList);
                }
            }
            else if (descriptor.ImplementationFactory != null)
            {
                // 这里是我不能理解的地方，不知道为啥要这样
                var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                {
                    var serviceProvider = context.Resolve<IServiceProvider>();
                    return descriptor.ImplementationFactory(serviceProvider);
                }).ConfigureLifecycle(descriptor.Lifetime)
                  .CreateRegistration();

                builder.RegisterComponent(registration);
            }
            else
            {
                builder
                    .RegisterInstance(descriptor.ImplementationInstance!)
                    .As(descriptor.ServiceType)
                    .ConfigureLifecycle(descriptor.Lifetime);
            }
        }
    }
}
