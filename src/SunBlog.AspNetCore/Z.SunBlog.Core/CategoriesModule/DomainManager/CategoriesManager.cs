using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.CategoriesModule.DomainManager
{
    public class CategoriesManager : BusinessDomainService<Categories>, ICategoriesManager
    {
        public CategoriesManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(Categories entity)
        {
            await Task.CompletedTask;
        }

    }
}
