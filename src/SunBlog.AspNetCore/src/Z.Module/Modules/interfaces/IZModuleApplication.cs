using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Module.Modules.interfaces
{
    public interface IZModuleApplication : IModuleContainer
    {
        Type StartupModuleType { get; }

        IServiceCollection Services { get; }

        IServiceProvider ServiceProvider { get; }

        void ConfigerService();

        void InitializeModules(IServiceProvider serviceProvider);

    }
}
