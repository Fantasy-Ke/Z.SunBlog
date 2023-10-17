﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Ddd.Common.DomainServiceRegister;
using Z.SunBlog.Application.OAuthModule.Dto;

namespace Z.SunBlog.Application.OAuthModule
{
    /// <summary>
    /// 第三方授权登录
    /// </summary>
    public interface IOAuthAppService : IApplicationService
    {
        Task<string> Get(string type);

        Task<IActionResult> Callback(string type, [FromQuery] string code, [FromQuery] string state);

        Task<string> Login(string code);

        Task<OAuthAccountDetailOutput> UserInfo();

        Task AddLink(AddLinkOutput dto);
    }
}
