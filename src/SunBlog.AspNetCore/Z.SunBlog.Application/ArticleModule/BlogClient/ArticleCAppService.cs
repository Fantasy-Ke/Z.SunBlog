using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.ArticleModule.BlogClient.Dto;
using Z.SunBlog.Core.ArticleCategoryModule.DomainManager;
using Z.SunBlog.Core.ArticleModule.DomainManager;
using Z.SunBlog.Core.ArticleTagModule.DomainManager;
using Z.SunBlog.Core.AuthAccountModule.DomainManager;
using Z.SunBlog.Core.CategoriesModule.DomainManager;
using Z.SunBlog.Core.FriendLinkModule.DomainManager;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.TagModule.DomainManager;

namespace Z.SunBlog.Application.ArticleModule.BlogClient
{
    /// <summary>
    /// ArticleCAppService文章前台管理
    /// </summary>
    public class ArticleCAppService : ApplicationService, IArticleCAppService
    {
        private readonly IArticleDomainManager _articleDomainManager;
        private readonly IArticleTagManager _articleTagManager;
        private readonly IArticleCategoryManager _articleCategoryManager;
        private readonly ICategoriesManager _categoriesManager;
        private readonly ITagsManager _tagsManager;
        private readonly IAuthAccountDomainManager _authAccountDomainManager;
        private readonly IFriendLinkManager _friendLinkManager;
        private readonly IPraiseManager _praiseManager;
        public ArticleCAppService(
            IServiceProvider serviceProvider, IArticleDomainManager articleDomainManager,
            IArticleTagManager articleTagManager, IArticleCategoryManager articleCategoryManager, ICategoriesManager categoriesManager, ITagsManager tagsManager, IPraiseManager praiseManager, IAuthAccountDomainManager authAccountDomainManager, IFriendLinkManager friendLinkManager) : base(serviceProvider)
        {
            _articleDomainManager = articleDomainManager;
            _articleTagManager = articleTagManager;
            _articleCategoryManager = articleCategoryManager;
            _categoriesManager = categoriesManager;
            _tagsManager = tagsManager;
            _praiseManager = praiseManager;
            _authAccountDomainManager = authAccountDomainManager;
            _friendLinkManager = friendLinkManager;
        }

        /// <summary>
        /// 文章栏目分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CategoryOutput>> Categories()
        {
            var queryable = _articleDomainManager.QueryAsNoTracking.Where(a => a.Status == AvailabilityStatus.Enable && a.PublishTime <= DateTime.Now && (a.ExpiredTime == null || DateTime.Now < a.ExpiredTime));

            return await _categoriesManager.QueryAsNoTracking.GroupJoin(
                _articleCategoryManager.QueryAsNoTracking,
                c => c.Id,
                ac => ac.CategoryId,
                (c, ac) => new { categories = c, articleCate = ac })
                .SelectMany(a => a.articleCate.DefaultIfEmpty(), (m, n) => new
                {
                    articleCate = n,
                    categories = m
                })
                .GroupJoin(
                queryable,
                ac => ac.articleCate.ArticleId,
                qa => qa.Id,
                (c, qa) => new { categories = c.categories.categories, articleCate = c.articleCate, queryable = qa })
                .SelectMany(a => a.queryable.DefaultIfEmpty(), (m, n) => new
                {
                    articleCate = m.articleCate,
                    categories = m.categories,
                    queryable =n
                })
                .Where(c => c.categories.Status == AvailabilityStatus.Enable)
                .GroupBy(c => new { c.categories.Id, c.categories.ParentId, c.categories.Name, c.categories.Sort })
                  .Select(c => new CategoryOutput
                  {
                      Id = c.Key.Id,
                      ParentId = c.Key.ParentId,
                      Sort = c.Key.Sort,
                      Name = c.Key.Name,
                      Total = c.Count()
                  })
                .OrderBy(x => x.Sort)
                .OrderBy(x => x.ParentId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        /// <summary>
        /// 文章表查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<ArticleOutput>> GetList([FromBody] ArticleListQueryInput dto)
        {
            if (dto.TagId.HasValue)
            {
                var tag = await _tagsManager.QueryAsNoTracking.Where(x => x.Id == dto.TagId && x.Status == AvailabilityStatus.Enable).Select(x => new { x.Name, x.Cover }).FirstOrDefaultAsync();
                HttpExtension.Fill(new { tag.Name, tag.Cover });
            }

            if (dto.CategoryId.HasValue)
            {
                var category = await _categoriesManager.QueryAsNoTracking.Where(x => x.Id == dto.CategoryId && x.Status == AvailabilityStatus.Enable).Select(x => new { x.Name, x.Cover }).FirstOrDefaultAsync();
                HttpExtension.Fill(new { category.Name, category.Cover });
            }

            var tagquery = from a in _tagsManager.QueryAsNoTracking
                        .Where(x => x.Status == AvailabilityStatus.Enable)
                           join ac in _articleTagManager.QueryAsNoTracking on
                                  a.Id equals ac.TagId
                           select new
                           {
                               tags = a,
                               at = ac
                           };


            var query = from a in _articleDomainManager.QueryAsNoTracking
                        .Where(x => x.PublishTime <= DateTime.Now && x.Status == AvailabilityStatus.Enable)
                        .Where(x => x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
                        .WhereIf(dto.TagId.HasValue,c => tagquery.Any(p=>p.at.ArticleId == c.Id && p.tags.Id == dto.TagId))
                        .WhereIf(!string.IsNullOrWhiteSpace(dto.Keyword), article => article.Title.Contains(dto.Keyword) || article.Summary.Contains(dto.Keyword) || article.Content.Contains(dto.Keyword))
                        join ac in _articleCategoryManager.QueryAsNoTracking
                        .WhereIf(dto.CategoryId.HasValue, ac => ac.CategoryId == dto.CategoryId) on
                              a.Id equals ac.ArticleId into category
                        from c in category.DefaultIfEmpty()

                        join cg in _categoriesManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable) on
                        c.CategoryId equals cg.Id

                        orderby a.IsTop descending
                        orderby a.Sort
                        orderby a.PublishTime

                        select new ArticleOutput
                        {
                            Id = a.Id,
                            Title = a.Title,
                            CategoryId = cg.Id,
                            CategoryName = cg.Name,
                            IsTop = a.IsTop,
                            CreationType = a.CreationType,
                            Summary = a.Summary,
                            Cover = a.Cover,
                            PublishTime = a.PublishTime,
                            Tags = _tagsManager.QueryAsNoTracking.Where(p => p.Status == AvailabilityStatus.Enable)
                            .Join(_articleTagManager.QueryAsNoTracking.Where(p => p.ArticleId == a.Id),
                            t => t.Id, at => at.TagId,
                            (t, at) => new { tags = t, articleTag = at })
                            .Select(p => new TagsOutput
                            {
                                Id = p.tags.Id,
                                Name = p.tags.Name,
                                Color = p.tags.Color,
                                Icon = p.tags.Icon
                            }).ToList()
                        };

            return  await query.OrderByDescending(p=>p.IsTop).ToPagedListAsync(dto);
        }

        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleInfoOutput> Info([FromQuery] Guid id)
        {
            var userId = UserService.UserId;
            var article = await (from a in _articleDomainManager.QueryAsNoTracking
                        .Where(x => x.Id == id && x.PublishTime <= DateTime.Now && x.Status == AvailabilityStatus.Enable)
                        .Where(x=>x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
                        join ac in _articleCategoryManager.QueryAsNoTracking on
                        a.Id equals ac.ArticleId into category
                        from c in category.DefaultIfEmpty()

                        join cg in _categoriesManager.QueryAsNoTracking.Where(x=>x.Status == AvailabilityStatus.Enable) on
                        c.CategoryId equals cg.Id 

                        select new ArticleInfoOutput
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content.Replace("\\n", "\n"),
                            Summary = a.Summary,
                            Cover = a.Cover,
                            PublishTime = a.PublishTime,
                            Author = a.Author,
                            Views = a.Views,
                            CreationType = a.CreationType,
                            IsAllowComments = a.IsAllowComments,
                            IsHtml = a.IsHtml,
                            IsTop = a.IsTop,
                            Link = a.Link,
                            UpdatedTime = a.UpdatedTime,
                            CategoryId = cg.Id,
                            PraiseTotal = _praiseManager.QueryAsNoTracking.Count(p => p.ObjectId == a.Id),
                            IsPraise = _praiseManager.QueryAsNoTracking.Where(p => p.ObjectId == a.Id && p.AccountId == userId).Any(),
                            CategoryName = cg.Name
                        }).FirstOrDefaultAsync();
            var tagsList = await (from t in _tagsManager.QueryAsNoTracking.Where(p=>p.Status == AvailabilityStatus.Enable)
                        join at in _articleTagManager.QueryAsNoTracking.Where(p => p.ArticleId == id)
                        on t.Id equals at.TagId
                        select new TagsOutput
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Color = t.Color,
                            Icon = t.Icon
                        }).ToListAsync();



            article!.Tags = tagsList;


            if (article == null) throw new UserFriendlyException("糟糕，您访问的信息丢失了...",404);

            var updateArt = await _articleDomainManager.FindByIdAsync(article.Id);

            updateArt!.Views += 1;

            await _articleDomainManager.UpdateAsync(updateArt);

            //上一篇
            var prevQuery = _articleDomainManager.QueryAsNoTracking.Where(x => x.PublishTime < article.PublishTime && x.PublishTime <= DateTime.Now && x.Status == AvailabilityStatus.Enable)
                .Where(x => x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
                 .OrderByDescending(x => x.PublishTime)
                 .Select(x => new ArticleBasicsOutput { Id = x.Id, Cover = x.Cover, Title = x.Title, PublishTime = null, Type = 0 }).Take(1);
            //下一篇
            var nextQuery = _articleDomainManager.QueryAsNoTracking.Where(x => x.PublishTime > article.PublishTime && x.PublishTime <= DateTime.Now && x.Status == AvailabilityStatus.Enable)
                .Where(x => x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
                    .OrderBy(x => x.PublishTime)
                    .Select(x => new ArticleBasicsOutput { Id = x.Id, Cover = x.Cover, Title = x.Title, PublishTime = null, Type = 1 }).Take(1);

            //随机6条
            var randomQuery = _articleDomainManager.QueryAsNoTracking.Where(x => x.Id != id)
                .Where(x => x.PublishTime <= DateTime.Now && x.Status == AvailabilityStatus.Enable)
                .Where(x => x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
                .OrderBy(x => x.CreationTime)
                .Select(x => new ArticleBasicsOutput
                { Id = x.Id, Cover = x.Cover, Title = x.Title, PublishTime = x.PublishTime, Type = 2 })
                .Take(6);


            //相关文章
            var relevant = await prevQuery.Concat(nextQuery).Concat(randomQuery).ToListAsync();
            article.Prev = relevant.FirstOrDefault(x => x.Type == 0)!;
            article.Next = relevant.FirstOrDefault(x => x.Type == 1)!;
            article.Random = relevant.Where(x => x.Type == 2).ToList();
            article.Views++;
            return article;
        }

        /// <summary>
        /// 最新5片文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ArticleBasicsOutput>> Latest()
        {
            var result = await _articleDomainManager.QueryAsNoTracking
              .Where(x => x.Status == AvailabilityStatus.Enable && x.PublishTime <= DateTime.Now)
              .Where(x => x.ExpiredTime == null || x.ExpiredTime > DateTime.Now)
              .Take(5)
              .OrderByDescending(x => x.Id)
              .ToListAsync();


            return ObjectMapper.Map<List<ArticleBasicsOutput>>(result);
        }

        /// <summary>
        /// 文章信息统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ArticleReportOutput> ReportStatistics()
        {
            //统计文章数量
            int articleCount = await _articleDomainManager.QueryAsNoTracking
                .Where(x => x.Status == AvailabilityStatus.Enable && (x.ExpiredTime == null || DateTime.Now < x.ExpiredTime))
                .Where(x => x.PublishTime <= DateTime.Now)
                .CountAsync();

            //标签统计
            int tagCount = await _tagsManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable).CountAsync();
            //栏目统计
            int categoryCount = await _categoriesManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable).CountAsync();

            int userCount = await _authAccountDomainManager.QueryAsNoTracking.CountAsync();

            int linkCount = await _friendLinkManager.QueryAsNoTracking.CountAsync(x => x.Status == AvailabilityStatus.Enable);

            return new ArticleReportOutput
            {
                ArticleCount = articleCount,
                CategoryCount = categoryCount,
                TagCount = tagCount,
                LinkCount = linkCount,
                UserCount = userCount
            };
        }

        /// <summary>
        /// 标签列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<TagsOutput>> Tags()
        {
            return await _tagsManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable)
                .OrderBy(x => x.Sort)
                .Select(x => new TagsOutput
                {
                    Id = x.Id,
                    Icon = x.Icon,
                    Name = x.Name,
                    Color = x.Color
                }).ToListAsync();
        }
    }


}
