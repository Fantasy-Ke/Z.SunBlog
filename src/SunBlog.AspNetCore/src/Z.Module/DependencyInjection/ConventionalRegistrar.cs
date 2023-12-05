using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Z.Module.Extensions;
using Z.Module.Reflection;

namespace Z.Module.DependencyInjection
{
    public class ConventionalRegistrar
    {

        public  void AddAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = AssemblyHelper
                .GetAllTypes(assembly)
                .Where(
                    type => type != null &&
                            type.IsClass &&
                            !type.IsAbstract &&
                            !type.IsGenericType
                ).ToArray();

            AddTypes(services, types);
        }


        public void AddType(IServiceCollection services, Type type)
        {

            var registerLifeAttribute = GetRegisterLifeAttributeOrNull(type);

            var lifeTime = GetLifeTimeOrNull(type, registerLifeAttribute);

            if (lifeTime == null)
            {
                return;
            }

            var exposedServiceTypes = GetDefaultServices(type);


            foreach (var exposedServiceType in exposedServiceTypes)
            {
                var serviceDescriptor = ServiceDescriptor.Describe(
                exposedServiceType,
                 type,
                lifeTime.Value
                );

                if (registerLifeAttribute?.ReplaceServices == true)
                {
                    services.Replace(serviceDescriptor);
                }
                else if (registerLifeAttribute?.TryRegister == true)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else
                {
                    services.Add(serviceDescriptor);
                }
            }
        }

        public  void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        protected virtual RegisterLifeAttribute GetRegisterLifeAttributeOrNull(Type type)
        {
            return type.GetCustomAttribute<RegisterLifeAttribute>(true);
        }


        protected ServiceLifetime? GetLifeTimeOrNull(Type type,  RegisterLifeAttribute registerLifeAttribute)
        {
            return registerLifeAttribute?.Lifetime ?? GetServiceLifetime(type);
        }

        protected  ServiceLifetime? GetServiceLifetime(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }

        private static List<Type> GetDefaultServices(Type type)
        {
            var serviceTypes = new List<Type>();

            serviceTypes.AddIfNotContains(type);

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;

                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }
    }
}
