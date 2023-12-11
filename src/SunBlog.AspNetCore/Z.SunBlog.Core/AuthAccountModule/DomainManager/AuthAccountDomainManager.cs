using Microsoft.EntityFrameworkCore;
using MrHuo.OAuth.QQ;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.OAuth.Gitee;
using Z.Fantasy.Core.OAuth.GitHub;

namespace Z.SunBlog.Core.AuthAccountModule.DomainManager
{
    public class AuthAccountDomainManager : BasicDomainService<AuthAccount, string>, IAuthAccountDomainManager
    {
        public AuthAccountDomainManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(AuthAccount entity)
        {
           await Task.CompletedTask;
        }

        public async Task<AuthAccount> CreateQQAccount(QQUserInfoModel qqInfo,string oauthId)
        {
            var account = await QueryAsNoTracking.FirstAsync(x => x.OAuthId == oauthId && x.Type.ToLower() == "qq");
            var gender = qqInfo.Gender == "男" ? Gender.Male :
                qqInfo.Gender == "女" ? Gender.Female : Gender.Unknown;
            if (account != null)
            {
                await UpdateAsync(new AuthAccount()
                {
                    Avatar = string.IsNullOrWhiteSpace(qqInfo.QQ100Avatar) ? qqInfo.Avatar : qqInfo.QQ100Avatar,
                    Name = qqInfo.Name,
                    IsBlogger = account.IsBlogger,
                    Gender = gender
                },
                    x => x.OAuthId == oauthId && x.Type.ToLower() == "qq");
            }
            else
            {
                account = await CreateAsync(new AuthAccount()
                {
                    Gender = gender,
                    Avatar = qqInfo.Avatar,
                    Name = qqInfo.Name,
                    OAuthId = oauthId,
                    Type = "QQ"
                });
            }

            return account;
        }

        public async Task<AuthAccount> CreateGiteeAccount(ZGiteeUserModel giteeInfo)
        {
            var  account = await QueryAsNoTracking.FirstOrDefaultAsync(x => x.OAuthId == giteeInfo.Id.ToString() && x.Type.ToLower() == "gitee");
            if (account != null)
            {
                await UpdateAsync(new AuthAccount()
                {
                    Avatar = giteeInfo.Avatar,
                    Name = giteeInfo.Name,
                    IsBlogger = account.IsBlogger,
                    Gender = Gender.Unknown
                },
                    x => x.OAuthId == giteeInfo.Id.ToString() && x.Type.ToLower() == "gitee");
            }
            else
            {
                account = await CreateAsync(new AuthAccount()
                {
                    Gender = Gender.Unknown,
                    Avatar = giteeInfo.Avatar,
                    Name = giteeInfo.Name,
                    OAuthId = giteeInfo.Id.ToString(),
                    Type = "Gitee"
                });
            }

            return account;
        }

        public async Task<AuthAccount> CreateGitHubAccount(ZGitHubUserModel githubInfo)
        {
            var account = await QueryAsNoTracking.FirstOrDefaultAsync(x => x.OAuthId == githubInfo.Id.ToString() && x.Type.ToLower() == "github");
            if (account != null)
            {
                await UpdateAsync(new AuthAccount()
                {
                    Avatar = githubInfo.Avatar,
                    Name = githubInfo.Name,
                    IsBlogger = account.IsBlogger,
                    Gender = Gender.Unknown
                },
                    x => x.OAuthId == githubInfo.Id.ToString() && x.Type.ToLower() == "github");
            }
            else
            {
                account = await CreateAsync(new AuthAccount()
                {
                    Gender = Gender.Unknown,
                    Avatar = githubInfo.Avatar,
                    Name = githubInfo.Name,
                    OAuthId = githubInfo.Id.ToString(),
                    Type = "GitHub"
                });
            }

            return account;
        }
    }
}
