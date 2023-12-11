namespace Z.Fantasy.Core.Authorization.Authorize
{
    public interface IAuthorizePermissionContext : IDisposable
    {
        SystemPermission DefinePermission { get; }
    }
}
