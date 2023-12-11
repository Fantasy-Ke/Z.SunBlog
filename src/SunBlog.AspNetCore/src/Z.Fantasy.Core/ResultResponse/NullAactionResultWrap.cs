using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.ResultResponse;

public class NullAactionResultWrap : IActionResultWarp
{
    public void Wrap(FilterContext context)
    {
        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                resultExecutingContext.Result = new ObjectResult(new ZEngineResponse());
                return;
        }


    }
}
