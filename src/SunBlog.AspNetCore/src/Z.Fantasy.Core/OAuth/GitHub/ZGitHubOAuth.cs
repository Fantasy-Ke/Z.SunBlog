using MrHuo.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.OAuth.GitHub
{
    public class ZGitHubOAuth : OAuthLoginBase<ZGitHubUserModel>
    {
        protected override string AuthorizeUrl => "https://github.com/login/oauth/authorize";

        protected override string AccessTokenUrl => "https://github.com/login/oauth/access_token";

        protected override string UserInfoUrl => "https://api.github.com/user";

        public ZGitHubOAuth(OAuthConfig oauthConfig)
            : base(oauthConfig)
        {
        }

        public override async Task<ZGitHubUserModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            ZGitHubUserModel githubUserModel = await HttpRequestApi.GetAsync<ZGitHubUserModel>(UserInfoUrl, BuildGetUserInfoParams(accessTokenModel), new Dictionary<string, string> { ["Authorization"] = "token " + accessTokenModel.AccessToken });
            if (githubUserModel.HasError())
            {
                throw new Exception(githubUserModel.ErrorMessage);
            }

            return githubUserModel;
        }
    }
}
