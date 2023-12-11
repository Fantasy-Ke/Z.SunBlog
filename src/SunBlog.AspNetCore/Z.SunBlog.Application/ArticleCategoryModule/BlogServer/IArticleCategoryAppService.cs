using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Application.ArticleCategoryModule.BlogServer.Dto;

namespace Z.SunBlog.Application.ArticleCategoryModule.BlogServer
{
    /// <summary>
    /// 文章所属栏目管理
    /// </summary>
    public interface IArticleCategoryAppService : IApplicationService
    {
        Task Create(CreateOrUpdateArticleCategoryDto dto);


        Task Update(CreateOrUpdateArticleCategoryDto dto);
    }
}
