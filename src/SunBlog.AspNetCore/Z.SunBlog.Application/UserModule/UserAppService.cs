using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Users;
using Z.Fantasy.Core.Extensions;
using Z.SunBlog.Application.UserModule.Dto;
using Z.SunBlog.Core.UserModule.DomainManager;
using System.ComponentModel;
using Z.Fantasy.Core.UserSession;
using Yitter.IdGenerator;
using Z.Fantasy.Core.Exceptions;

namespace Z.SunBlog.Application.UserModule
{
    /// <summary>
    /// 用户服务
    /// </summary>
	public class UserAppService : ApplicationService, IUserAppService
    {
        public readonly IUserDomainManager _userDomainManager;
        private readonly IIdGenerator _idGenerator;
        public UserAppService(IUserDomainManager userDomainManager,
            IServiceProvider serviceProvider,
            IIdGenerator idGenerator) : base(serviceProvider)
        {
            _userDomainManager = userDomainManager;
            _idGenerator = idGenerator;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public async Task<ZUserInfo> Create()
        {
            var dfs = await _userDomainManager.QueryAsNoTracking.FirstOrDefaultAsync();

            //await _userDomainManager.DeleteAsync("6e37cc6e9b1948dba987d07b25ffc138");
            //await _userDomainManager.DeleteAsync("acab70064e8a45a0bef2074b42d9165e");
            var df1 = ObjectMapper.Map<ZUserInfoDto>(dfs);
            HttpExtension.Fill(new { df1.UserName, df1.PassWord });
            return dfs;
        }

        /// <summary>
        /// 查询一个用户
        /// </summary>
        /// <returns></returns>
        public async Task<List<ZUserInfoDto>> GetFrist()
        {
            var dfs = await _userDomainManager.QueryAsNoTracking.ToListAsync();

            return  ObjectMapper.Map<List<ZUserInfoDto>>(dfs);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public async Task<ZUserInfoDto?> Login(ZUserInfoDto user)
		{
			var dfs = await _userDomainManager.QueryAsNoTracking.FirstOrDefaultAsync(P=>P.UserName == user.UserName);


            
            if (dfs == null || dfs.PassWord != MD5Encryption.Encrypt($"{_idGenerator.Encode(dfs.Id)}{user.PassWord}"))
                return default;

            return ObjectMapper.Map<ZUserInfoDto>(dfs);
		}


        [DisplayName("获取登录用户的信息")]
        [HttpGet]
        public async Task<ZUserInfoOutput> CurrentUserInfo()
        {
            var userId = UserService.UserId;
            if (userId.IsNullWhiteSpace()) return null;
            return await _userDomainManager.QueryAsNoTracking.Where(x => x.Id == userId)
                  .Select(x => new ZUserInfoOutput
                  {
                      Name = x.Name,
                      UserName = x.UserName,
                      Avatar = x.Avatar,
                      Birthday = x.Birthday,
                      Email = x.Email,
                      Gender = x.Gender,
                      LastLoginIp = x.LastLoginIp,
                      LastLoginAddress = x.LastLoginAddress,
                      Mobile = x.Mobile,
                      OrgId = x.OrgId,
                      //OrgName = SqlFunc.Subqueryable<SysOrganization>().Where(o => o.Id == x.OrgId).Select(o => o.Name)
                  })
                  //.Mapper(dto =>
                  //{
                  //    if (_authManager.IsSuperAdmin)
                  //    {
                  //        dto.AuthBtnList = _repository.AsSugarClient().Queryable<SysMenu>().Where(x => x.Type == MenuType.Button)
                  //              .Select(x => x.Code).ToList();
                  //    }
                  //    else
                  //    {
                  //        var list = _sysMenuService.GetAuthButtonCodeList(userId).GetAwaiter().GetResult();
                  //        dto.AuthBtnList = list.Where(x => x.Access).Select(x => x.Code).ToList();
                  //    }
                  //})
                  .FirstOrDefaultAsync() ?? new ZUserInfoOutput();
        }

    }


}
