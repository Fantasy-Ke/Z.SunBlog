using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResultExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ResultExecutingContext;
using Microsoft.AspNetCore.Http;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;

namespace Z.Fantasy.Core.ResultResponse;

public class JsonActionResultWrap : IActionResultWarp
{
    public void Wrap(FilterContext context)
    {

        JsonResult jsonResult = null;

        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                jsonResult = resultExecutingContext.Result as JsonResult;
                break;
        }

        if (jsonResult == null)
        {
            throw new UserFriendlyException("Action Result should be JsonResult!");
        }

        var statusCode = context.HttpContext.Response.StatusCode;

        if (!(jsonResult.Value is ZResponseBase))
        {
            var isSuccess = statusCode == StatusCodes.Status200OK;
            var response = new ZEngineResponse();
            if (isSuccess) { response.Result = jsonResult.Value; } else { response.Error = new ErrorInfo { Message = jsonResult.Value }; }
            response.Success = isSuccess;
            response.StatusCode = statusCode;
            response.Extras = HttpExtension.Take();
            jsonResult.Value = response;
        }
    }
}
