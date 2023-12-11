using MrHuo.OAuth.QQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Users;
using Z.Fantasy.Core.OAuth.Gitee;
using Z.Fantasy.Core.OAuth.GitHub;

namespace Z.SunBlog.Core.AuthAccountModule.DomainManager
{
    public interface IAuthAccountDomainManager : IBasicDomainService<AuthAccount, string>
    {

        Task<AuthAccount> CreateQQAccount(QQUserInfoModel qqInfo, string oauthId);

        Task<AuthAccount> CreateGiteeAccount(ZGiteeUserModel giteeInfo);

        Task<AuthAccount> CreateGitHubAccount(ZGitHubUserModel githubInfo);
    }
}
