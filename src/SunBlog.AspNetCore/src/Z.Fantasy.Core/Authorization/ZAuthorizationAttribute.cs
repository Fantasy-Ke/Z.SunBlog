using Microsoft.AspNetCore.Authorization;

namespace Z.Fantasy.Core.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ZAuthorizationAttribute : AuthorizeAttribute
{
    public virtual string[] AuthorizeName { get; private set; }
    public virtual bool IsMethodValidation { get; private set; } = false;

    public ZAuthorizationAttribute(params string[] authorizeName)
    {
        AuthorizeName = authorizeName;
        Policy = string.Join(",", AuthorizeName);
    }
}
