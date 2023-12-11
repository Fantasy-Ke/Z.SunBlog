using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.FriendLinkModule;

/// <summary>
/// 友情链接
/// </summary>
public class FriendLink : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 博客用户Id
    /// </summary>
    public string? AppUserId { get; set; }

    /// <summary>
    /// 网站名称
    /// </summary>
    [MaxLength(32)]
    public string SiteName { get; set; }

    /// <summary>
    /// 网站链接
    /// </summary>
    [MaxLength(256)]
    public string Link { get; set; }

    /// <summary>
    /// 网站logo
    /// </summary>
    [MaxLength(256)]
    public string Logo { get; set; }

    /// <summary>
    /// 对方博客友链的地址
    /// </summary>
    [MaxLength(256)]
    public string? Url { get; set; }

    /// <summary>
    /// 是否忽略对方站点是否存在本站链接
    /// </summary>
    public bool IsIgnoreCheck { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

}