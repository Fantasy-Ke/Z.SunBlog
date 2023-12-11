using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.MenuModule.DomainManager
{
    public class MenuManager : BusinessDomainService<Menu>, IMenuManager
    {
        public MenuManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(Menu entity)
        {
            await Task.CompletedTask;
        }

    }
}
