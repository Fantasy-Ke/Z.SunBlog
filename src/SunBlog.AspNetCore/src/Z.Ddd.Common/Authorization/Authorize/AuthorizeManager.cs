using CSRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Yitter.IdGenerator;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.Entities.Permission;
using Z.Ddd.Common.Entities.Repositories;
using Z.Ddd.Common.Entities.Users;
using Z.Ddd.Common.RedisModule;

namespace Z.Ddd.Common.Authorization.Authorize
{
    public class AuthorizeManager : IAuthorizeManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizeRegister _authorizeRegister;
        private readonly IServiceProvider _serviceProvider;

        public AuthorizeManager(
            ICacheManager cacheManager,
            IConfiguration configuration,
            IAuthorizeRegister authorizeRegister,
            IServiceProvider serviceProvider
        )
        {
            _cacheManager = cacheManager;
            _configuration = configuration;
            _authorizeRegister = authorizeRegister;
            _serviceProvider = serviceProvider;
        }

        public async Task AddAuthorizeRegiester(IAuthorizePermissionContext Context)
        {
            var key = _configuration.GetSection("App:Permission").Get<string>();

            var providers = _authorizeRegister.AuthorizeProviders;
            foreach (var provider in providers)
            {
                provider.PermissionDefined(Context);
            }
            var permissions = InitPermission(Context);
            Context.Dispose();
            try
            {
                using var scope = _serviceProvider.CreateAsyncScope();
                var baseService = scope.ServiceProvider.GetRequiredService<IBasicRepository<ZPermissions, string>>();
                if (await baseService.CountAsync(p => true) > 0)
                {
                    await baseService.DeleteManyAsync();
                    await baseService.InsertManyAsync(permissions);
                    await _cacheManager.RemoveCacheAsync(key);
                    await _cacheManager.SetCacheAsync(key, permissions);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        private List<ZPermissions> InitPermission(IAuthorizePermissionContext context)
        {
            List<ZPermissions> permissions = new List<ZPermissions>();
            ZPermissions permission;

            if (context.DefinePermission.Childrens == null) goto end;

            foreach (var childer in context.DefinePermission.Childrens)
            {
                permission = new ZPermissions(
                    childer.Name,
                    childer.Code,
                    childer.ParentCode,
                    childer.IsGroup,
                    childer.Page,
                    childer.Button
                );

                permissions.Add(permission);

                InitChilder(permissions, childer.Childrens);

                childer.Dispose();
            }
        end:
            return permissions;
        }

        private void InitChilder(List<ZPermissions> permissions, List<SystemPermission> systemPermissions)
        {
            if (systemPermissions is null)
            {
                return;
            }
            ZPermissions permission;
            foreach (var item in systemPermissions)
            {
                permission = new ZPermissions(
                    item.Name,
                    item.Code,
                    item.ParentCode,
                    item.IsGroup,
                    item.Page,
                    item.Button
                );

                permissions.Add(permission);

                InitChilder(permissions, item.Childrens);

                item.Dispose();
            }
        }
    }
}
