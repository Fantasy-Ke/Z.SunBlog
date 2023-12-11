using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Entities.Users;
using Z.SunBlog.Core.ArticleCategoryModule;

namespace Z.SunBlog.Core.ArticleCategoryModule.DomainManager
{
    public interface IArticleCategoryManager : IBusinessDomainService<ArticleCategory>
    {
    }
}
