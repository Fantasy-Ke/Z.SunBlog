using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Yitter.IdGenerator;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Organizations;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.Entities.Users;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.RedisModule;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.Fantasy.Core.UserSession;
using Z.EntityFrameworkCore.Extensions;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.ConfigModule;
using Z.SunBlog.Application.SystemServiceModule.UserService.Dto;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.CustomConfigModule;
using Z.SunBlog.Core.UserModule.DomainManager;

namespace Z.SunBlog.Application.SystemServiceModule.UserService
{
    public interface IUserSysAppService : IApplicationService, ITransientDependency
    {
        Task<PageResult<UserPageOutput>> PageData([FromBody] QueryUserInput dto);

        Task AddUser(AddUserInput dto);

        Task UpdateUser(UpdateUserInput dto);

        Task<UpdateUserInput> Detail([FromQuery] string id);

        Task Reset(ResetPasswordInput dto);

        Task<UserInfoOutput> CurrentUserInfo();

        Task UploadAvatar([FromBody] string url);

        Task Delete(string id);

        Task UpdateCurrentUser(UpdateCurrentUserInput dto);
    }
    /// <summary>
    /// 用户操作服务
    /// </summary>
    public class UserSysAppService : ApplicationService, IUserSysAppService
    {
        private readonly IBasicRepository<ZOrganization, string> _orgRepository;
        private readonly IBasicRepository<ZUserRole> _userRoleRepository;
        private readonly IIdGenerator _idGenerator;
        private readonly ICustomConfigAppService _customConfigService;
        private readonly ICacheManager _cacheManager;

        private readonly IUserDomainManager _userDomainManager;
        public UserSysAppService(IServiceProvider serviceProvider,
            IBasicRepository<ZOrganization, string> orgRepository,
            IUserDomainManager userDomainManager,
            IIdGenerator idGenerator,
            ICustomConfigAppService customConfigService,
            IBasicRepository<ZUserRole> userRoleRepository,
            ICacheManager cacheManager) : base(serviceProvider)
        {
            _orgRepository = orgRepository;
            _userDomainManager = userDomainManager;
            _idGenerator = idGenerator;
            _customConfigService = customConfigService;
            _userRoleRepository = userRoleRepository;
            _cacheManager = cacheManager;
        }

        private async Task<List<ZOrganization>> GetChildOrg(string id, List<ZOrganization> orgLists)
        {
            var orgList = _orgRepository.GetQueryAll().Where(p => p.ParentId == id);
            if (await orgList.AnyAsync())
            {
                orgLists.AddRange(orgList);
                foreach (var org in orgLists)
                {
                    return await GetChildOrg(org.Id, orgLists);
                }
            }
            return orgLists;
        }

        /// <summary>
        /// 系统用户分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("系统用户分页查询")]
        [HttpPost]
        public async Task<PageResult<UserPageOutput>> PageData([FromBody] QueryUserInput dto)
        {
            List<string> orgIdList = new List<string>();
            if (!string.IsNullOrWhiteSpace(dto.OrgId))
            {
                orgIdList.Add(dto.OrgId);
                var orgEntity = await _orgRepository.GetQueryAll().Include(p => p.Children).FirstOrDefaultAsync(p => p.Id == dto.OrgId);
                if (orgEntity != null)
                {
                    var list = await GetChildOrg(orgEntity.Id, new List<ZOrganization>());
                    orgIdList.AddRange(list.Select(x => x.Id));
                }
            }
            return await _userDomainManager.QueryAsNoTracking
                .WhereIf(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(dto.UserName), x => x.UserName.Contains(dto.UserName))
                .WhereIf(!string.IsNullOrWhiteSpace(dto.Mobile), x => x.Mobile.Contains(dto.Mobile))
                .WhereIf(orgIdList.Any(), x => orgIdList.Contains(x.OrgId))
                .Select(x => new UserPageOutput
                {
                    Name = x.Name,
                    Status = x.Status,
                    UserName = x.UserName,
                    Birthday = x.Birthday,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    NickName = x.Name,
                    CreatedTime = x.CreationTime,
                    Email = x.Email,
                    Id = x.Id
                }).ToPagedListAsync(dto);
        }

        /// <summary>
        /// 添加系统用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("添加系统用户")]
        public async Task AddUser(AddUserInput dto)
        {
            var user = ObjectMapper.Map<ZUserInfo>(dto);
            user.Id = _idGenerator.NextId();
            string encode = _idGenerator.Encode(user.Id);
            var setting = await _customConfigService.Get<SysSecuritySetting>();
            user.PassWord = MD5Encryption.Encrypt(encode + (setting?.Password ?? "123456"));
            var roles = dto.Roles?.Select(x => new ZUserRole()
            {
                RoleId = x,
                UserId = user.Id
            }).ToList();
            await _userDomainManager.CreateAsync(user);
            await _userRoleRepository.InsertManyAsync(roles);
        }

        /// <summary>
        /// 更新系统用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("更新系统用户信息")]
        public async Task UpdateUser(UpdateUserInput dto)
        {
            var user = await _userDomainManager.FindByIdAsync(dto.Id);
            if (user == null) throw new UserFriendlyException("无效参数");

            ObjectMapper.Map(dto, user);
            var roles = dto.Roles?.Select(x => new ZUserRole()
            {
                RoleId = x,
                UserId = user.Id
            }).ToList();
            await _userDomainManager.UpdateAsync(user);
            await _userRoleRepository.DeleteAsync(x => x.UserId == user.Id);
            if (roles!=null && roles.Any())
            {
                await _userRoleRepository.InsertManyAsync(roles);
            }
            await _cacheManager.RemoveByPrefixAsync(CacheConst.PermissionKey);
        }

        /// <summary>
        /// 系统用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UpdateUserInput> Detail([FromQuery] string id)
        {
            return await _userDomainManager.QueryAsNoTracking.Where(x => x.Id == id)
                  .Select(x => new UpdateUserInput()
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Status = x.Status,
                      OrgId = x.OrgId,
                      UserName = x.UserName,
                      Mobile = x.Mobile,
                      Birthday = x.Birthday,
                      Email = x.Email,
                      Gender = x.Gender,
                      Roles = _userRoleRepository.GetQueryAll().Where(s => s.UserId == x.Id).Select(p => p.RoleId).ToList()
                  }).FirstAsync();
        }

        /// <summary>
        /// 重置系统用户密码
        /// </summary>
        /// <returns></returns>
        [DisplayName("重置系统用户密码")]
        [HttpPatch]
        public async Task Reset(ResetPasswordInput dto)
        {
            string encrypt = MD5Encryption.Encrypt(_idGenerator.Encode(dto.Id) + dto.Password);
            await _userDomainManager.UpdateAsync(new ZUserInfo()
            {
                PassWord = encrypt
            }, x => x.Id == dto.Id);
        }

        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <returns></returns>
        [DisplayName("获取登录用户的信息")]
        [HttpGet]
        public async Task<UserInfoOutput> CurrentUserInfo()
        {
            var userId = UserService.UserId;
            return await _userDomainManager.QueryAsNoTracking.Where(x => x.Id == userId)
                  .Select(x => new UserInfoOutput
                  {
                      Id = x.Id,
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
                      OrgName = _orgRepository.GetQueryAll().Where(o => o.Id == x.OrgId).Select(o => o.Name).FirstOrDefault()
                  })
                  .FirstAsync();
        }

        /// <summary>
        /// 用户修改账户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("用户修改账户密码")]
        [HttpPatch]
        public async Task ChangePassword(ChangePasswordOutput dto)
        {
            var userId = UserService.UserId;
            string encode = _idGenerator.Encode(userId);
            string pwd = MD5Encryption.Encrypt($"{encode}{dto.OriginalPwd}");
            if (!await _userDomainManager.QueryAsNoTracking.AnyAsync(x => x.Id == userId && x.PassWord == pwd))
            {
                throw new UserFriendlyException("原密码错误");
            }
            pwd = MD5Encryption.Encrypt($"{encode}{dto.Password}");
            await _userDomainManager.UpdateAsync(
                new ZUserInfo()
                {
                    PassWord = pwd
                }, x => x.Id == userId);
        }

        /// <summary>
        /// 用户修改头像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [DisplayName("用户修改头像")]
        [HttpPatch]
        public async Task UploadAvatar([FromBody] string url)
        {
            var userId = UserService.UserId;
            await _userDomainManager.UpdateAsync(new ZUserInfo()
            {
                Avatar = url
            }, x => x.Id == userId);
        }

        /// <summary>
        /// 系统用户修改自己的信息
        /// </summary>
        /// <returns></returns>
        [DisplayName("系统用户修改个人信息")]
        [HttpPatch("updateCurrentUser")]
        public async Task UpdateCurrentUser(UpdateCurrentUserInput dto)
        {
            var userId = UserService.UserId;
            await _userDomainManager.UpdateAsync(new ZUserInfo()
            {
                Name = dto.Name,
                Birthday = dto.Birthday,
                Email = dto.Email,
                Gender = dto.Gender,
                Mobile = dto.Mobile,
            }, x => x.Id == userId);
        }


        [DisplayName("删除菜单/按钮"), HttpDelete]
        public async Task Delete(string id)
        {
            await _userDomainManager.DeleteAsync(id);
            await _cacheManager.RemoveByPrefixAsync(CacheConst.PermissionKey);
        }
    }
}
