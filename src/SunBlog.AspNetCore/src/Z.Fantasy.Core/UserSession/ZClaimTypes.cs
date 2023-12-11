
using System.Security.Claims;

namespace Z.Fantasy.Core.UserSession;

/// <summary>
/// Used to get Z-specific claim type names.
/// </summary>
public static class ZClaimTypes
{
    /// <summary>
    /// Default: <see cref="ClaimTypes.Name"/>
    /// </summary>
    public static string UserName { get; set; } = ClaimTypes.Name;

    /// <summary>
    /// Default: <see cref="ClaimTypes.Expiration"/>
    /// </summary>
    public static string Expiration { get; set; } = ClaimTypes.Expiration;

    /// <summary>
    /// Default: <see cref="RoleIds"/>
    /// </summary>
    public static string RoleIds { get; set; } = "RoleIds";

    /// <summary>
    /// Default: <see cref="ClaimTypes.NameIdentifier"/>
    /// </summary>
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

    /// <summary>
    /// Default: <see cref="ClaimTypes.Role"/>
    /// </summary>
    public static string Role { get; set; } = ClaimTypes.Role;
}
