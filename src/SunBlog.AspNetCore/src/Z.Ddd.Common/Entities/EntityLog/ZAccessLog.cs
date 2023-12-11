using System.ComponentModel.DataAnnotations;
using Z.Ddd.Common.Entities.Auditing;

namespace Z.Ddd.Common.Entities.EntityLog;
/// <summary>
/// 用户登录日志
/// </summary>
public class ZAccessLog : CreationAuditedEntity<Guid>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 访问路由
    /// </summary>
    public string RoutePath { get; set; }
    /// <summary>
    /// 访问参数（如：query/params参数信息）
    /// </summary>
    public string RouteParams { get; set; }

    /// <summary>
    /// ip地址
    /// </summary>

    [MaxLength(64)]
    public string? RemoteIp { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [MaxLength(256)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Ip归属地
    /// </summary>
    [MaxLength(128)]
    public string? Location { get; set; }

    /// <summary>
    /// 用户系统信息
    /// </summary>
    [MaxLength(256)]
    public string? OsDescription { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    [MaxLength(256)]
    public string? Message { get; set; }

    /// <summary>
    /// 操作类型（1登陆，2登出）
    /// </summary>
    public string LogType { get; set; }
}