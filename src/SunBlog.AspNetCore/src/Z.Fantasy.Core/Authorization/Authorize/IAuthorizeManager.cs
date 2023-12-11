using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization.Authorize
{
    public interface IAuthorizeManager : ITransientDependency
    {
        Task AddAuthorizeRegiester(IAuthorizePermissionContext AuthorizePermissionContext);
    }
}
