using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.SunBlog.Core.TagModule;

namespace Z.SunBlog.Core.TagModule.DomainManager
{
    public class TagsManager : BusinessDomainService<Tags>, ITagsManager
    {
        public TagsManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(Tags entity)
        {
            await Task.CompletedTask;
        }

    }
}
