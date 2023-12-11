using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.CustomConfigModule.DomainManager
{
    public class CustomConfigManager : BusinessDomainService<CustomConfig>, ICustomConfigManager
    {
        public CustomConfigManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(CustomConfig entity)
        {
            await Task.CompletedTask;
        }

    }
}
