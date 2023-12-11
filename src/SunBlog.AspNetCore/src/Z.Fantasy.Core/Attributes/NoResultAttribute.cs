using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class NoResultAttribute : Attribute
{
}
