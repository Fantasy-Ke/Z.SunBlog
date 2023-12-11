using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Authorization;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization
{
    public class ZAuthorizationPolicyProvider : IAuthorizationPolicyProvider, ITransientDependency
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddAuthenticationSchemes("Bearer");
            policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
            //默认授权测率必须添加一个IAuthorizationRequirement的实现
            policy.AddRequirements(new AuthorizeRequirement());
            return Task.FromResult(policy.Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddAuthenticationSchemes("Bearer");
            policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
            if (policyName is null)
            {
                return Task.FromResult<AuthorizationPolicy>(null);
            }
            var authorizations = policyName.Split(',');
            if (authorizations.Any())
            {
                policy.AddRequirements(new AuthorizeRequirement(authorizations));
            }
            return Task.FromResult(policy.Build())!;
        }
    }
}
