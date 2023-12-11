using Microsoft.EntityFrameworkCore;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Application.ArticleCategoryModule.BlogServer.Dto;
using Z.SunBlog.Core.ArticleCategoryModule;
using Z.SunBlog.Core.ArticleCategoryModule.DomainManager;

namespace Z.SunBlog.Application.ArticleCategoryModule.BlogServer
{
    /// <summary>
    /// ArticleCategoryAppService文章所属栏目管理
    /// </summary>
    public class ArticleCategoryAppService : ApplicationService, IArticleCategoryAppService
    {
        private readonly IArticleCategoryManager _articleCategoryManager;
        public ArticleCategoryAppService(
            IServiceProvider serviceProvider, IArticleCategoryManager articleCategoryManager) : base(serviceProvider)
        {
            _articleCategoryManager = articleCategoryManager;
        }

        /// <summary>
        /// 添加文章所属栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task Create(CreateOrUpdateArticleCategoryDto dto)
        {
            await _articleCategoryManager.CreateAsync(new ArticleCategory()
            {
                CategoryId = dto.CategoryId,
                ArticleId = dto.ArticleId,
            });
        }

        /// <summary>
        /// 更新文章所属栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task Update(CreateOrUpdateArticleCategoryDto dto)
        {
            var articleCategory = await _articleCategoryManager.QueryAsNoTracking.FirstOrDefaultAsync(p=>p.ArticleId == dto.ArticleId);

            articleCategory!.CategoryId = dto.CategoryId;

            await _articleCategoryManager.UpdateAsync(articleCategory);
        }
    }


}
