using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.SystemServiceModule.UserService.Dto;

public class QueryUserInput : Pagination
{
    /// <summary>
    /// 账号
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 组织机构Id
    /// </summary>
    public string? OrgId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }
}