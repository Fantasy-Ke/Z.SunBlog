using Autofac;

namespace Z.Fantasy.Core.AutofacExtensions
{
    /// <summary>
    /// 注入的服务容器
    /// </summary>
    public class IOCManager
    {
        /// <summary>
        /// autofac容器
        /// </summary>
        public static IContainer Current { get; set; } = default!;

        public static T GetService<T>() => Current.Resolve<T>();

        public static bool IsRegistered<T>() => Current.IsRegistered<T>();

        public static bool IsRegistered<T>(string key) => Current.IsRegisteredWithKey<T>(key);
        public static bool IsRegisteredWithName<T>(string serviceName) => Current.IsRegisteredWithName<T>(serviceName);

        public static bool IsRegistered(Type type) => Current.IsRegistered(type);

        public static bool IsRegisteredWithKey(string key, Type type) => Current.IsRegisteredWithKey(key, type);
        public static bool IsRegisteredWithName(string serviceName, Type type) => Current.IsRegisteredWithName(serviceName, type);

        public static T GetService<T>(string key) => Current.ResolveKeyed<T>(key);
        public static T GetServiceByName<T>(string serviceName) => Current.ResolveNamed<T>(serviceName);

        public static object GetService(Type type) => Current.Resolve(type);

        public static object GetService(string key, Type type) => Current.ResolveKeyed(key, type);
        public static object GetServiceByName(string serviceName, Type type) => Current.ResolveNamed(serviceName, type);
        public static T GetServiceByName<T>(string serviceName, params NamedParameter[] parameters) => Current.ResolveNamed<T>(serviceName, parameters);
        public static T GetService<T>(params NamedParameter[] parameters) => Current.Resolve<T>(parameters);

    }
}
