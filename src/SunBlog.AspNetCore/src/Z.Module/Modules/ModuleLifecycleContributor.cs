using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public abstract class ModuleLifecycleContributor : IModuleLifecycleContributor
    {
        public virtual void Initialize(InitApplicationContext context, IZModule module)
        {
        }

        public virtual Task InitializeAsync(InitApplicationContext context, IZModule module)
        {
            return Task.CompletedTask;
        }
    }
}
