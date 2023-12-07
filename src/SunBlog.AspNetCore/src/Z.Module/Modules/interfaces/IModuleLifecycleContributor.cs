using Z.Module.DependencyInjection;

namespace Z.Module.Modules.interfaces
{
    public interface IModuleLifecycleContributor : ITransientDependency
    {
        void Initialize(InitApplicationContext context, IZModule module);

        Task InitializeAsync(InitApplicationContext context, IZModule module);
    }
}
