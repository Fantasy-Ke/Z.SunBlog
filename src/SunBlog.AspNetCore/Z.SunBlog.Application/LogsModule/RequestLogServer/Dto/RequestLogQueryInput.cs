using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.LogsModule.RequestLogServer.Dto;

public class RequestLogQueryInput : Pagination
{
    /// <summary>
    /// 关键词
    /// </summary>
    public string name { get; set; }
}