
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.UserSession
{
    public interface IUserSession
    {
        public string UserId { get; }

        public string UserName { get; }

        public IEnumerable<string>? RoleName { get; }

        public IEnumerable<string>? RoleIds { get; }

        Claim? FindClaim(string claimType);

        Claim[] FindClaims(string claimType);
    }
}
