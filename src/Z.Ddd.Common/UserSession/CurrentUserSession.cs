﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;

namespace Z.Ddd.Common.UserSession
{
    public class CurrentUserSession : IUserSession, ITransientDependency
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly ClaimsPrincipal? _principal;
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        public CurrentUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _principal = _httpContextAccessor.HttpContext?.User;
        }

        public virtual string UserId => this.FindClaimValue(ZClaimTypes.UserId) ?? string.Empty;

        public virtual string UserName => this.FindClaimValue(ZClaimTypes.UserName) ?? string.Empty;

        public virtual IEnumerable<string>? RoleName => FindClaims(ZClaimTypes.Role).Select(c => c.Value).Distinct().ToArray();

        public virtual IEnumerable<string>? RoleIds => FindClaims(ZClaimTypes.RoleIds).Select(c => c.Value).Distinct().ToArray();

        public virtual Claim? FindClaim(string claimType)
        {
            return _principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return _principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

    }
}
