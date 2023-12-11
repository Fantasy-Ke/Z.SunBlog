using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.UserSession;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization
{
    public class ZAuthorizationHandler : AuthorizationHandler<AuthorizeRequirement>, ISingletonDependency
    {
        private readonly IPermissionCheck _permisscheck;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationSchemeProvider _authenticationSchemes;

        public ZAuthorizationHandler(IAuthenticationSchemeProvider authenticationSchemes, IHttpContextAccessor httpContextAccessor, IPermissionCheck permisscheck)
        {
            _authenticationSchemes = authenticationSchemes;
            _httpContextAccessor = httpContextAccessor;
            _permisscheck = permisscheck;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
        {
            var identity = _httpContextAccessor?.HttpContext?.User?.Identity;
            var httpContext = _httpContextAccessor?.HttpContext;
            var isAuthenticated = identity?.IsAuthenticated ?? false;
            var claims = _httpContextAccessor?.HttpContext?.User?.Claims;
            var userId = claims?.FirstOrDefault(p => p.Type == ZClaimTypes.UserId)?.Value;
            var schemes = await _authenticationSchemes.GetAllSchemesAsync();
            var handlers = httpContext?.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in schemes)
            {
                //判断请求是否停止
                if (handlers?.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler requestHandler && await requestHandler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            AuthorizationFailureReason failureReason;
            //判断是否通过鉴权中间件--是否登录
            if (userId is null || !isAuthenticated)
            {
                failureReason = new AuthorizationFailureReason(this, "请登录到系统");
                context.Fail(failureReason);
                return;
            }
            //判断token是否过期
            DateTime expire;
            var expiraton = claims?.FirstOrDefault(p => p.Type == ClaimTypes.Expiration);
            DateTime.TryParse(expiraton?.Value ?? "", out expire);
            if (expire < DateTime.Now)
            {
                failureReason = new AuthorizationFailureReason(this, "Token过期，请重新登陆");
                context.Fail(failureReason);
                return;
            }

            var defaultPolicy = requirement.AuthorizeName?.Any() ?? false;
            //默认授权策略
            if (!defaultPolicy)
            {
                context.Succeed(requirement);
                return;
            }
            var roleIds = claims?
                .Where(p => p?.Type?.Equals("RoleIds") ?? false)
                .Select(p => p.Value);
            var roleNames = claims?
                .Where(p => p?.Type?.Equals(ClaimTypes.Role) ?? false)
                .Select(p => p.Value);
            UserTokenModel tokenModel = new UserTokenModel()
            {
                UserId = userId ?? "0",
                UserName = claims?.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "",
                RoleNames = roleNames?.ToArray(),
                RoleIds = roleIds?.ToArray(),
            };
            if (requirement.AuthorizeName!.Any())
            {
                if (!_permisscheck.IsGranted(tokenModel, requirement.AuthorizeName!))
                {
                    failureReason = new AuthorizationFailureReason(this, $"权限不足，无法请求--请求接口{httpContext?.Request?.Path ?? ""}");
                    context.Fail(failureReason);
                    return;
                }
            }
            context.Succeed(requirement);
        }
    }
}
