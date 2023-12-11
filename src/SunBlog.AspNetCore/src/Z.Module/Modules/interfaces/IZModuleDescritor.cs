using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Z.Module.Modules.interfaces
{
    public interface IZModuleDescritor
    {
        Type Type { get; }

        Assembly Assembly { get; }

        public Assembly[] AllAssemblies { get; }

        IZModule Instance { get; }
    }
}
