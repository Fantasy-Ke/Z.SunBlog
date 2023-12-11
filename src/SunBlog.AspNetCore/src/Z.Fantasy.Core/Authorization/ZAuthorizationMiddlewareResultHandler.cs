using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Z.Fantasy.Core.ResultResponse;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization
{
    public class ZAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler, ISingletonDependency
    {
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded || authorizeResult.Challenged)
            {
                var reason = authorizeResult?.AuthorizationFailure?.FailureReasons.FirstOrDefault();
                var isLogin = context?.User?.Identity?.IsAuthenticated ?? false;
                var path = context?.Request?.Path ?? "";
                context!.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var response = new ZEngineResponse(new ErrorInfo
                {
                    Message = isLogin ? reason?.Message : "请先登录系统"
                }, true);
                response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(response);
                return;
            }
            await next(context);
        }
    }
}
