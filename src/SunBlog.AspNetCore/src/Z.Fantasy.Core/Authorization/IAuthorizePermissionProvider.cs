using Z.Fantasy.Core.Authorization.Authorize;

namespace Z.Fantasy.Core.Authorization
{
    public interface IAuthorizePermissionProvider
    {
        void PermissionDefined(IAuthorizePermissionContext context);
    }
}
