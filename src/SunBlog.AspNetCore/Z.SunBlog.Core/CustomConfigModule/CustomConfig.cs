
using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.CustomConfigModule;
/// <summary>
/// 自定义配置
/// </summary>
public class CustomConfig : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 配置名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 配置唯一编码（类名）
    /// </summary>
    [MaxLength(32)]
    public string Code { get; set; }

    /// <summary>
    /// 是否是多项（集合）
    /// </summary>
    public bool IsMultiple { get; set; }

    /// <summary>
    /// 配置界面设计（json）
    /// </summary>
    [MaxLength(int.MaxValue)]
    public string? Json { get; set; }

    /// <summary>
    /// 是否允许创建实体
    /// </summary>
    public bool AllowCreationEntity { get; set; }

    /// <summary>
    /// 是否已生成实体
    /// </summary>
    public bool IsGenerate { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }
}