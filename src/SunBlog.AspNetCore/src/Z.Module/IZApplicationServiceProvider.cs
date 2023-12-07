using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules.interfaces;

namespace Z.Module
{
    public interface IZApplicationServiceProvider : IZModuleApplication
    {
        void Initialize(IServiceProvider serviceProvider);
        Task InitializeAsync(IServiceProvider serviceProvider);
    }
}
