using MrHuo.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.OAuth.Gitee
{
    public class ZGiteeOAuth : OAuthLoginBase<ZGiteeUserModel>
    {
        protected override string AuthorizeUrl => "https://gitee.com/oauth/authorize";

        protected override string AccessTokenUrl => "https://gitee.com/oauth/token";

        protected override string UserInfoUrl => "https://gitee.com/api/v5/user";

        public ZGiteeOAuth(OAuthConfig oauthConfig)
            : base(oauthConfig)
        {
        }

        public override async Task<ZGiteeUserModel> GetUserInfoAsync(DefaultAccessTokenModel accessTokenModel)
        {
            ZGiteeUserModel giteeUserModel = await HttpRequestApi.GetAsync<ZGiteeUserModel>(UserInfoUrl, BuildGetUserInfoParams(accessTokenModel));
            if (giteeUserModel.HasError())
            {
                throw new Exception(giteeUserModel.ErrorMessage);
            }

            return giteeUserModel;
        }
    }
}
