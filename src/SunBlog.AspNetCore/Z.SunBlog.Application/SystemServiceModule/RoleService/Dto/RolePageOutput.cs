using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.SystemServiceModule.RoleService.Dto;

public class RolePageOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }


    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 排序值
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}