using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.FriendLinkModule.DomainManager
{
    public class FriendLinkManager : BusinessDomainService<FriendLink>, IFriendLinkManager
    {
        public FriendLinkManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(FriendLink entity)
        {
            await Task.CompletedTask;
        }

    }
}
