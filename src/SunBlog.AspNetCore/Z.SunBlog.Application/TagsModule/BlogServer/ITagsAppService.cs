using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.TagsModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.TagsModule.BlogServer
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface ITagsAppService : IApplicationService
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
        Task<PageResult<TagsPageOutput>> GetPage([FromBody] TagsPageQueryInput dto);

        /// <summary>
        /// 创建修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateTagInput dto);

        /// <summary>
        /// 文章标签下拉选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<List<SelectOutput>> Select();

    }
}
