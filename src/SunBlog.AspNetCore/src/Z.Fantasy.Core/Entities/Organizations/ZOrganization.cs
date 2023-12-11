

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.Fantasy.Core.Entities.Organizations;

/// <summary>
/// 组织机构信息表
/// </summary>
public class ZOrganization : FullAuditedEntity<string>
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 部门编码
    /// </summary>
    [MaxLength(64)]
    public string Code { get; set; }

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
    public string Remark { get; set; }

    /// <summary>
    /// 子部门
    /// </summary>
    [NotMapped]
    public virtual ZOrganization Parent { get; set; }

    /// <summary>
    /// 子部门
    /// </summary>
    [NotMapped]
    public virtual List<ZOrganization> Children { get; set; }
}