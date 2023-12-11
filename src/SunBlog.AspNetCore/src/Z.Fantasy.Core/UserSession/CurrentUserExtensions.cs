using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.UserSession
{
    public static class CurrentUserExtensions
    {
        public static string FindClaimValue(this IUserSession currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }
    }
}
