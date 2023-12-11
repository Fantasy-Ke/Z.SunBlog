using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.AlbumsModule.BlogClient.Dto;
using Z.SunBlog.Application.ArticleModule.BlogClient.Dto;
using Z.SunBlog.Application.TalksModule.BlogClient.Dto;

namespace Z.SunBlog.Application.TalksModule.BlogClient
{
    /// <summary>
    /// 博客说说
    /// </summary>
    public interface ITalksCAppService : IApplicationService
    {
        Task<PageResult<TalksOutput>> GetList([FromBody] Pagination dto);

        Task<TalkDetailOutput> TalkDetail([FromQuery] Guid id);
    }
}
