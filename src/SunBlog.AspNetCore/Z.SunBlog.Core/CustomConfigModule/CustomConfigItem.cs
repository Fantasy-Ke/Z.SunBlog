using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.CustomConfigModule;
/// <summary>
/// 自定义配置子项
/// </summary>
public class CustomConfigItem : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 自定义配置Id
    /// </summary>
    public Guid ConfigId { get; set; }

    /// <summary>
    /// 配置
    /// </summary>
    [MaxLength(int.MaxValue)]
    public string Json { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
}