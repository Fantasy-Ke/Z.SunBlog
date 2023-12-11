using Z.SunBlog.Core.AlbumsModule;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.AlbumsModule.BlogServer.Dto;

public class AlbumsPageOutput
{
    /// <summary>
    /// 相册ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 相册类型
    /// </summary>
    public CoverType? Type { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsVisible { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    /// 封面
    /// </summary>
    public string Cover { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}