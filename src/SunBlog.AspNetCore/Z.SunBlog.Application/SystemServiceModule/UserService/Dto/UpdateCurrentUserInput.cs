using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.SystemServiceModule.UserService.Dto;

public class UpdateCurrentUserInput
{
    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "姓名为必填项")]
    [MaxLength(16, ErrorMessage = "姓名限制16个字符内")]
    public string? Name { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(32, ErrorMessage = "昵称限制32个字符")]
    public string? NickName { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [MaxLength(16, ErrorMessage = "手机号码限制16个字符内")]
    public string? Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(64, ErrorMessage = "邮箱限制64个字符内")]
    public string? Email { get; set; }
}