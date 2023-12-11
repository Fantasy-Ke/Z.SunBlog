using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.Fantasy.Core.Entities.Roles;

/// <summary>
/// 角色表
/// </summary>
public class ZRoleInfo : FullAuditedEntity<string>
{
    /// <summary>
    /// 角色名
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    [MaxLength(32)]
    public string Code { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string Remark { get; set; }
}
