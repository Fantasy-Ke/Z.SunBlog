using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.Fantasy.Core.Entities.EntityLog;

public class ZRequestLog : CreationAuditedEntity<Guid>
{
    /// <summary>
    /// 请求URI
    /// </summary>
    [MaxLength(256)]
    public string RequestUri { get; set; }
    /// <summary>
    /// 请求方式
    /// </summary>
    [MaxLength(50)]
    public string RequestType { get; set; }
    /// <summary>
    /// 请求数据
    /// </summary>
    [MaxLength(2560)]
    public string RequestData { get; set; }
    /// <summary>
    /// 响应数据
    /// </summary>
    [MaxLength(int.MaxValue)]
    public string ResponseData { get; set; }
    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(50)]
    public string UserId { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    [MaxLength(50)]
    public string UserName { get; set; }

    /// <summary>
    /// 访问ip
    /// </summary>
    [MaxLength(50)]
    public string ClientIP { get; set; }
    /// <summary>
    /// 用户代理（主要指浏览器）
    /// </summary>
    [MaxLength(50)]
    public string UserAgent { get; set; }
    /// <summary>
    /// 操作系统
    /// </summary>
    [MaxLength(50)]
    public string UserOS { get; set; }
    /// <summary>
    /// 耗时
    /// </summary>
    [MaxLength(50)]
    public string SpendTime { get; set; }
}
