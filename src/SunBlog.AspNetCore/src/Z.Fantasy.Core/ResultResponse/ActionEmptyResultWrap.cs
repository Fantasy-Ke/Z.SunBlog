using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Z.Fantasy.Core.Extensions;

namespace Z.Fantasy.Core.ResultResponse;

public class ActionEmptyResultWrap : IActionResultWarp
{
    public void Wrap(FilterContext context)
    {
        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                var statusCode = context.HttpContext.Response.StatusCode;
                var isSuccess = statusCode == StatusCodes.Status200OK;
                resultExecutingContext.Result = new ObjectResult(new ZEngineResponse(statusCode, isSuccess));
                return;
        }
    }
}
