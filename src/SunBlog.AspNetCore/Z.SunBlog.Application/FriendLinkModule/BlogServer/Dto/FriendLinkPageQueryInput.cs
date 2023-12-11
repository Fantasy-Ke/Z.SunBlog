using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.FriendLinkModule.BlogServer.Dto;

public class FriendLinkPageQueryInput : Pagination
{
    /// <summary>
    /// 站点名称
    /// </summary>
    public string? SiteName { get; set; }
}