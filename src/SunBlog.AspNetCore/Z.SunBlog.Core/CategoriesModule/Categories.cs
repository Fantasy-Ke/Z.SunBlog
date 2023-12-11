
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.CategoriesModule;

/// <summary>
/// 文章栏目表
/// </summary>
public class Categories : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 栏目名称
    /// </summary>
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 父级id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [MaxLength(256)]
    public string Cover { get; set; }

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
    /// 子栏目
    /// </summary>
    [NotMapped]
    public List<Categories> Children { get; set; } = new();
}