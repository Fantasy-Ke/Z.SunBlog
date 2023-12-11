using Z.Fantasy.Core.DomainServiceRegister.Domain;

namespace Z.SunBlog.Core.ArticleTagModule.DomainManager
{
    public class ArticleTagManager : BusinessDomainService<ArticleTag>, IArticleTagManager
    {
        public ArticleTagManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(ArticleTag entity)
        {
            await Task.CompletedTask;
        }

    }
}
