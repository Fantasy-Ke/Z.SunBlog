namespace Z.Fantasy.Core.Authorization.Authorize
{
    public abstract class AuthorizePermissionProvider : IAuthorizePermissionProvider
    {
        public abstract void PermissionDefined(IAuthorizePermissionContext context);
    }
}
