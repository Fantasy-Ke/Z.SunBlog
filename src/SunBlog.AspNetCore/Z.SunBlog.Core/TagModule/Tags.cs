using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.TagModule;

/// <summary>
/// 标签信息表
/// </summary>
public class Tags : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 标签名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [MaxLength(256)]
    public string Cover { get; set; }

    /// <summary>
    /// 标签颜色
    /// </summary>
    [MaxLength(64)]
    public string? Color { get; set; }

    [MaxLength(32)]
    public string? Icon { get; set; }

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
    public string? Remark { get; set; }

}