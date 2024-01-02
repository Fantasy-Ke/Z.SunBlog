using System.ComponentModel.DataAnnotations;

namespace Z.SunBlog.Application.LogsModule.RequestLogServer.Dto;

public class RequestLogOutput
{
    public Guid Id { get; set; }
    /// <summary>
    /// 请求URI
    /// </summary>
    public string RequestUri { get; set; }
    /// <summary>
    /// 请求方式
    /// </summary>
    public string RequestType { get; set; }
    /// <summary>
    /// 请求数据
    /// </summary>
    public string RequestData { get; set; }
    /// <summary>
    /// 响应数据
    /// </summary>
    public string ResponseData { get; set; }
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 访问ip
    /// </summary>
    public string ClientIP { get; set; }
    /// <summary>
    /// 用户代理（主要指浏览器）
    /// </summary>
    public string UserAgent { get; set; }
    /// <summary>
    /// 操作系统
    /// </summary>
    public string UserOS { get; set; }
    /// <summary>
    /// 耗时
    /// </summary>
    public string SpendTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreationTime { get; set; }
}