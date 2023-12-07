using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;
using Z.Module.Extensions;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public abstract class ZModuleApplicationBase : IZModuleApplication
    {
        public Type StartupModuleType { get; }

        public IServiceCollection Services { get; private set; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IReadOnlyList<IZModuleDescritor> Modules { get; private set; }

        internal ZModuleApplicationBase(Type startupModuleType, IServiceCollection services)
        {
            var moduleLoader = new ModuleLoader();
            ZModule.CheckModuleType(startupModuleType);
            services.CheckNull();
            Services = services;
            StartupModuleType = startupModuleType;
            services.AddSingleton<IModuleLoader>(moduleLoader);
            services.AddObjectAccessor<IServiceProvider>();
            services.AddObjectAccessor<InitApplicationContext>();
            Services.AddSingleton<IModuleContainer>(this);
            Services.AddAssemblyOf<IZModuleApplication>();
            services.Configure<ZModuleLifecycleOptions>(options =>
            {
                options.Contributors.Add<OnInitApplicationModuleLifecycleContributor>();
                options.Contributors.Add<PostInitApplicationModuleLifecycleContributor>();
            });

            Modules = LoadModules(services);

            ConfigerService();
            
        }

        /// <summary>
        /// 服务生命周期
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void ConfigerService()
        {
            var context = new ServiceConfigerContext(Services);

            ////模块里的ServiceConfigerContext赋值
            foreach (var module in Modules)
            {
                if (module.Instance is ZModule abpModule)
                {
                    abpModule.ServiceConfigerContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    module.Instance.PreConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"期间发生错误 {nameof(IPreConfigureServices.PreConfigureServices)} 模块阶段 {module.Type.AssemblyQualifiedName}.有关详细信息，请参阅内部异常。.", ex);
                }
            }

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is ZModule zModule)
                {
                    if (!zModule.SkipAutoServiceRegistration)
                    {
                        //继承生命周期接口的类进行自动注册
                        Services.AddAssembly(module.Type.Assembly);
                    }
                }

                try
                {
                    module.Instance.ConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"期间发生错误  {nameof(IZModule.ConfigureServices)}  模块阶段  {module.Type.AssemblyQualifiedName}.有关详细信息，请参阅内部异常。", ex);
                }
            }
            //清空模块里的ServiceConfigerContext
            foreach (var module in Modules)
            {
                if (module.Instance is ZModule abpModule)
                {
                    abpModule.ServiceConfigerContext = null;
                }
            }


        }

        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        /// <summary>
        /// 应用程序的生命周期
        /// </summary>
        public void InitializeModules(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            ServiceProvider.GetRequiredService<IObjectAccessor<InitApplicationContext>>().Value = 
                new InitApplicationContext(scope.ServiceProvider);

            scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModules();
        }


        /// <summary>
        /// 模块加载
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected virtual IReadOnlyList<IZModuleDescritor> LoadModules(IServiceCollection services)
        {
            //找到IModule实例使用GetZModuleDescritors方法
            return services
                .GetSingletonInstance<IModuleLoader>()
                .GetZModuleDescritors(
                    services,
                    StartupModuleType
                );
        }
    }
}
