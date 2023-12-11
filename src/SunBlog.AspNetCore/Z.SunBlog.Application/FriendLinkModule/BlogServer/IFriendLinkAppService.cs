using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.FriendLinkModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.FriendLinkModule.BlogServer
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface IFriendLinkAppService : IApplicationService
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task DeleteAsync(KeyDto dto);

        /// <summary>
        /// 文章列表分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<FriendLinkPageOutput>> GetPage([FromBody] FriendLinkPageQueryInput dto);

        /// <summary>
        /// 创建修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateFriendInput dto);

    }
}
