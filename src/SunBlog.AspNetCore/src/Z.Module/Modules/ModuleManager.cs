using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public class ModuleManager : IModuleManager
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly IEnumerable<IModuleLifecycleContributor> _moduleLifecycleContributors;
        private readonly IObjectAccessor<InitApplicationContext> _applicationContext;

        public ModuleManager(IModuleContainer moduleContainer
            , IServiceProvider serviceProvider,
                IOptions<ZModuleLifecycleOptions> options,
                IObjectAccessor<InitApplicationContext> applicationContext)
        {
            _moduleContainer = moduleContainer;
            _moduleLifecycleContributors = options.Value
               .Contributors
                .Select(serviceProvider.GetRequiredService)
                .Cast<IModuleLifecycleContributor>()
                .ToArray();
            _applicationContext = applicationContext;
        }

        public void InitializeModules()
        {
            foreach (var contributor in _moduleLifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        contributor.Initialize(_applicationContext.Value, module.Instance);
                    }

                    catch (Exception ex)
                    {
                        throw new ArgumentException($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }

        public async Task InitializeModulesAsync()
        {
            foreach (var contributor in _moduleLifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        contributor.Initialize(_applicationContext.Value, module.Instance);

                        await contributor.InitializeAsync(_applicationContext.Value, module.Instance);
                        
                    }

                    catch (Exception ex)
                    {
                        throw new ArgumentException($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }
    }
}
