using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Users;
using Z.SunBlog.Application.UserModule.Dto;

namespace Z.SunBlog.Application.UserModule
{
    public interface IUserAppService : IApplicationService
    {
        Task<ZUserInfo> Create();

        Task<List<ZUserInfoDto>> GetFrist();

		Task<ZUserInfoDto?> Login(ZUserInfoDto user);

        Task<ZUserInfoOutput> CurrentUserInfo();

    }
}
