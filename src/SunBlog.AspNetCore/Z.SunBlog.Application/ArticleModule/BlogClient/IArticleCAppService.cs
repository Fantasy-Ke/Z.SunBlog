using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.ArticleModule.BlogClient.Dto;

namespace Z.SunBlog.Application.ArticleModule.BlogClient
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface IArticleCAppService : IApplicationService
    {
        Task<PageResult<ArticleOutput>> GetList([FromBody] ArticleListQueryInput dto);

        Task<List<TagsOutput>> Tags();

        Task<List<CategoryOutput>> Categories();

        Task<ArticleReportOutput> ReportStatistics();

        Task<ArticleInfoOutput> Info([FromQuery] Guid id);

        Task<List<ArticleBasicsOutput>> Latest();

    }
}
