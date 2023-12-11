using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.AuthAccountModule;
/// <summary>
/// 博客授权用户
/// </summary>
public class AuthAccount : FullAuditedEntity<string>
{
    /// <summary>
    /// 授权唯一标识
    /// </summary>
    public string OAuthId { get; set; }

    /// <summary>
    /// 授权类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 博主标识
    /// </summary>
    public bool IsBlogger { get; set; }

    /// <summary>
    /// 姓名（昵称）
    /// </summary>
    [MaxLength(64)]
    public string? Name { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [MaxLength(256)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(128)]
    public string? Email { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }

}