using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Authorization.Dtos;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Application.OAuthModule.Dto;

namespace Z.SunBlog.Application.OAuthModule
{
    /// <summary>
    /// 第三方授权登录
    /// </summary>
    public interface IOAuthAppService : IApplicationService
    {
        Task<string> GetIpAddress(string type);

        Task<IActionResult> Callback(string type, string code, string state);

        Task<ZFantasyToken> Login(string code);

        Task<OAuthAccountDetailOutput> UserInfo();

        Task AddLink(AddLinkOutput dto);

        Task<List<FriendLinkOutput>> Links();

        Task<BlogOutput> Info();
    }
}
