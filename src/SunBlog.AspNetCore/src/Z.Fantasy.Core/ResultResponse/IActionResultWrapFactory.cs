using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.ResultResponse;

public interface IActionResultWrapFactory : ITransientDependency
{
    IActionResultWarp CreateContext(FilterContext filterContext);
}
