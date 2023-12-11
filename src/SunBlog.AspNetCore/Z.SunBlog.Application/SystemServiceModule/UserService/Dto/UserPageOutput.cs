using Z.Fantasy.Core.Entities;
using Z.Fantasy.Core.Entities.Enum;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.SystemServiceModule.UserService.Dto;

public class UserPageOutput:Entity<string?>
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 账户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }
}