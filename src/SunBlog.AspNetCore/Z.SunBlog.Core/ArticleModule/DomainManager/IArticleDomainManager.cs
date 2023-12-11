using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Entities.Users;

namespace Z.SunBlog.Core.ArticleModule.DomainManager
{
    public interface IArticleDomainManager : IBusinessDomainService<Article>
    {
    }
}
