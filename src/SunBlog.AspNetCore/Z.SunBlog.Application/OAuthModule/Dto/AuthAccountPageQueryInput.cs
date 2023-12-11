using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.OAuthModule.Dto;

public class AuthAccountPageQueryInput : Pagination
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string? Name { get; set; }
}