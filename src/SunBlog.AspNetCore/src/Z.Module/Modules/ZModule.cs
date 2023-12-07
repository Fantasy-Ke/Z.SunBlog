using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public abstract class ZModule : IZModule
    {
        private ServiceConfigerContext _ServiceConfigerContext;


        /// <summary>
        /// 跳过自动注册
        /// </summary>
        protected internal bool SkipAutoServiceRegistration { get; protected set; }

        protected internal ServiceConfigerContext ServiceConfigerContext { get
            {
                if (_ServiceConfigerContext == null)
                {
                    throw new ArgumentNullException($"{nameof(ServiceConfigerContext)}不能没有值, 因为在 {nameof(PreConfigureServices)} 、 {nameof(ConfigureServices)} 、 {nameof(PostInitApplication)} 这几个方法中使用了");
                }
                return _ServiceConfigerContext;
            }
            internal set => _ServiceConfigerContext = value;
        }


        public virtual void PreConfigureServices(ServiceConfigerContext context)
        {
        }
        public virtual void ConfigureServices(ServiceConfigerContext context)
        {
        }

        public virtual void OnInitApplication(InitApplicationContext context)
        {
        }

        public virtual void PostInitApplication(InitApplicationContext context)
        {
        }
       
        /// <summary>
        /// 验证是否是继承ZMoudule的类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsZModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IZModule).GetTypeInfo().IsAssignableFrom(type);
        }


        protected void Configure<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigerContext.Services.Configure(configureOptions);
        }

        internal static void CheckModuleType(Type type)
        {
            if (!IsZModule(type))
            {
                throw new ArgumentNullException($"{type.Name}没有继承ZModule");
            }
        }

        public virtual Task OnInitApplicationAsync(InitApplicationContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task PostInitApplicationAsync(InitApplicationContext context)
        {
            return Task.CompletedTask;
        }
    }
}
