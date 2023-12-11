using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.AlbumsModule;

/// <summary>
/// 相册表
/// </summary>
public class Albums : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 相册名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [MaxLength(256)]
    public string Cover { get; set; }

    /// <summary>
    /// 相册类型
    /// </summary>
    public CoverType? Type { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible { get; set; }

}