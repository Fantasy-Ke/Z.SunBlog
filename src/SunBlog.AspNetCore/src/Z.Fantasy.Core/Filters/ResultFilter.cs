
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Z.Fantasy.Core.Attributes;
using Z.Fantasy.Core.ResultResponse;

namespace Z.Fantasy.Core.Filters;

public class ResultFilter : IResultFilter
{
    private readonly IActionResultWrapFactory _actionResultWrapFactory;

    public ResultFilter(IActionResultWrapFactory actionResultWrapFactory)
    {
        _actionResultWrapFactory = actionResultWrapFactory;
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var action = context.ActionDescriptor as ControllerActionDescriptor;
        var controllerAttribute = context.Controller
            .GetType()?
            .CustomAttributes?
            .FirstOrDefault(p => p.AttributeType == typeof(NoResultAttribute));
        if (controllerAttribute != null)
        {
            return;
        }
        var method = action?.MethodInfo;
        var nonResult = method?
            .CustomAttributes?
            .Where(p => p.AttributeType == typeof(NoResultAttribute));
        if (nonResult is null || !nonResult.Any())
        {
            _actionResultWrapFactory.CreateContext(context).Wrap(context);
        }
    }
}
