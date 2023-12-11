using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Extensions;
using Z.Module.Modules.interfaces;

namespace Z.Module.Modules
{
    public class ZModuleDescritor : IZModuleDescritor
    {
        public Type Type { get; }

        public Assembly Assembly { get; }

        public Assembly[] AllAssemblies { get; }

        public IZModule Instance { get; }

        public ZModuleDescritor(Type type,IZModule instance)
        {
            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }
            Type = type;
            Instance = instance;
            Assembly = type.Assembly;
            AllAssemblies = ZModuleHelper.GetAllAssemblies(type);
        }

    }
}
