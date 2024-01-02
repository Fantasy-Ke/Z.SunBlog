using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;

public class ExceptionlogQueryInput : Pagination
{
    /// <summary>
    /// 关键词
    /// </summary>
    public string name { get; set; }
}