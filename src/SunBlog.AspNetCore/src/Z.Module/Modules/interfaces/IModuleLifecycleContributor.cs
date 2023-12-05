using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;

namespace Z.Module.Modules.interfaces
{
    public interface IModuleLifecycleContributor : ITransientDependency
    {
        void Initialize(InitApplicationContext context, IZModule module);
    }
}
