﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Z.Foundation.Core.Exceptions;
using Z.Foundation.Core.Extensions;

namespace Z.Fantasy.Core.ResultResponse;

public class ObjectAactionResultWarp : IActionResultWarp
{
    public void Wrap(FilterContext context)
    {
        ObjectResult objectResult = null;

        switch (context)
        {
            case ResultExecutingContext resultExecutingContext:
                objectResult = resultExecutingContext.Result as ObjectResult;
                break;
        }

        if (objectResult == null)
        {
            throw new UserFriendlyException("Action Result should be JsonResult!");
        }

        var statusCode = context.HttpContext.Response.StatusCode;

        if (!(objectResult.Value is ZResponseBase))
        {
            var isSuccess = statusCode == StatusCodes.Status200OK;
            var response = new ZEngineResponse();
            if (isSuccess) { response.Result = objectResult.Value; } else { response.Error = new ErrorInfo { Message = objectResult.Value }; }
            response.Success = isSuccess;
            response.StatusCode = statusCode;
            response.Extras = HttpExtension.Take();
            objectResult.Value = response;
            objectResult.DeclaredType = typeof(ZEngineResponse);
        }
    }
}
