using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.TalksModule.DomainManager
{
    public class TalksManager : BusinessDomainService<Talks>, ITalksManager
    {
        public TalksManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(Talks entity)
        {
            await Task.CompletedTask;
        }

    }
}
