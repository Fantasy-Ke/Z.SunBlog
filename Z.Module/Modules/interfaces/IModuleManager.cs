﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Module.Modules.interfaces
{
    public interface IModuleManager
    {
        void InitializeModules([NotNull] InitApplicationContext context);
    }
}
