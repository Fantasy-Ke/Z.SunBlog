using System.Reflection;

namespace Z.Module.Modules;

public interface IAdditionalModuleAssemblyProvider
{
    Assembly[] GetAssemblies();
}