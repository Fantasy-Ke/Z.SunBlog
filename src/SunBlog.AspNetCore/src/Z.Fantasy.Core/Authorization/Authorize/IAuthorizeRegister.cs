using Z.Fantasy.Core.AutoMapper;

namespace Z.Fantasy.Core.Authorization.Authorize
{
    public interface IAuthorizeRegister
    {
        List<IAuthorizePermissionProvider> AuthorizeProviders { get; }
    }
}
