using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Z.Fantasy.Core.Authorization.Authorize
{
    public class AuthorizeRegister : IAuthorizeRegister
    {
        public List<IAuthorizePermissionProvider> AuthorizeProviders { get; private set; }
        public IServiceCollection Services { get; private set; }

        private static object obj = new object();

        public static AuthorizeRegister Register { get; private set; }

        private AuthorizeRegister()
        {
            AuthorizeProviders = new List<IAuthorizePermissionProvider>();
        }

        public void Init(IServiceCollection service)
        {
            Services = service;
            Services.AddSingleton<IAuthorizeRegister>(this);
        }
        static AuthorizeRegister()
        {
            if (Register != null)
            {
                return;
            }
            lock (obj)
            {
                if (Register == null)
                {
                    Register = new AuthorizeRegister();
                }
            }
        }

        public virtual void RegisterAuthorizeProvider<T>() where T : IAuthorizePermissionProvider
        {
            var instance = CreateInstance<T>();
            AuthorizeProviders.Add(instance);
        }

        public virtual void RegisterAuthorize<T>() where T : IAuthorizePermissionProvider
        {
            var instance = CreateInstance<T>();
            AuthorizeProviders.Add(instance);
        }

        private T CreateInstance<T>()
        {
            var tye = typeof(T);
            var newExpre = Expression.New(tye);
            var instance = Expression.Lambda<Func<T>>(newExpre).Compile();
            return instance.Invoke();
        }
    }
}