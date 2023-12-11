using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.CategoryModule.BlogServer.Dto;

public class CategoryPageOutput
{
    /// <summary>
    /// 栏目ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 栏目名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
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
    public string Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 子栏目
    /// </summary>
    public List<CategoryPageOutput> Children { get; set; } = new();
}