using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public class PostInitApplicationModuleLifecycleContributor : ModuleLifecycleContributor
    {
        public override void Initialize(InitApplicationContext context,IZModule module)
        {
            (module as IPostInitApplication)?.PostInitApplication(context);
        }

        public override async Task InitializeAsync(InitApplicationContext context, IZModule module)
        {
            await (module as IPostInitApplicationAsync)?.PostInitApplicationAsync(context);
        }
    }

    public class OnInitApplicationModuleLifecycleContributor : ModuleLifecycleContributor
    {
        public override void Initialize(InitApplicationContext context, IZModule module)
        {
            (module as IOnInitApplication)?.OnInitApplication(context);
        }

        public override async Task InitializeAsync(InitApplicationContext context, IZModule module)
        {
            await (module as IOnInitApplicationAsync)?.OnInitApplicationAsync(context);
        }
    }
}
