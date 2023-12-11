using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.CustomConfigModule.DomainManager
{
    public class CustomConfigItemManager : BusinessDomainService<CustomConfigItem>, ICustomConfigItemManager
    {
        public CustomConfigItemManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(CustomConfigItem entity)
        {
            await Task.CompletedTask;
        }

    }
}
