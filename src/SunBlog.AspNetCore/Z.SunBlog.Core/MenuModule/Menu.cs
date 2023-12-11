
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.MenuModule;

/// <summary>
/// 系统菜单表
/// </summary>
public class Menu : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType Type { get; set; }

    /// <summary>
    /// 权限编码
    /// </summary>
    [MaxLength(128)]
    public string? Code { get; set; }

    /// <summary>
    /// 父级菜单
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 路由名
    /// </summary>
    [MaxLength(32)]
    public string? RouteName { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [MaxLength(128)]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [MaxLength(128)]
    public string? Component { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    [MaxLength(128)]
    public string? Redirect { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    [MaxLength(64)]
    public string? Icon { get; set; }

    /// <summary>
    /// 是否内嵌页面
    /// </summary>
    public bool IsIframe { get; set; }

    /// <summary>
    /// 外链地址
    /// </summary>
    [MaxLength(256)]
    public string? Link { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    public bool IsKeepAlive { get; set; }

    /// <summary>
    /// 是否固定
    /// </summary>
    public bool IsFixed { get; set; }

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
    /// 子菜单
    /// </summary>
    [NotMapped]
    public List<Menu> Children { get; set; } = new();
}