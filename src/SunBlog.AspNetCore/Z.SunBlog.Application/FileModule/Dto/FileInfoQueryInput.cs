using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.FileModule.Dto;

public class FileInfoQueryInput : Pagination
{
    /// <summary>
    /// 关键词
    /// </summary>
    public string name { get; set; }
}