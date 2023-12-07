using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules;

namespace Z.Module
{
    /// <summary>
    /// 模块初始化Provider
    /// </summary>
    public class ZModuleApplicationServiceProvider : ZModuleApplicationBase, IZApplicationServiceProvider
    {
        public ZModuleApplicationServiceProvider(Type startModuleType, IServiceCollection services) : base(startModuleType, services)
        {
            services.AddSingleton<IZApplicationServiceProvider>(this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Initialize(IServiceProvider serviceProvider)
        {
            SetServiceProvider(serviceProvider);
            InitializeModules(serviceProvider);
        }
    }
}
