using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.MenuModule.DomainManager
{
    public class ZRoleMenuManager : BusinessDomainService<ZRoleMenu>, IZRoleMenuManager
    {
        public ZRoleMenuManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(ZRoleMenu entity)
        {
            await Task.CompletedTask;
        }

    }
}
