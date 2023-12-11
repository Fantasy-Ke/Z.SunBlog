using System.Security.Claims;

namespace Z.Fantasy.Core.Authorization;

public class UserTokenModel
{
    public virtual string UserId { get; set; }
    public virtual string UserName { get; set; }

    public virtual string[] RoleIds { get; set; }

    public virtual string[] RoleNames { get; set; }

    public virtual Claim[] Claims { get; set; }

}
