using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.SystemServiceModule.RoleService.Dto;

public class AddRoleInput
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [Required(ErrorMessage = "角色名称为必填项")]
    [MaxLength(32, ErrorMessage = "角色名称限制32个字符内")]
    public string? Name { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    [Required(ErrorMessage = "角色编码为必填项")]
    [MaxLength(32, ErrorMessage = "角色名称限制32个字符内")]
    public string? Code { get; set; }

    /// <summary>
    /// 排序值
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注限制200个字符内")]
    public string? Remark { get; set; }

    /// <summary>
    /// 授权按钮菜单Id
    /// </summary>
    [Required(ErrorMessage = "请为角色分配权限")]
    public List<Guid> Menus { get; set; }
}