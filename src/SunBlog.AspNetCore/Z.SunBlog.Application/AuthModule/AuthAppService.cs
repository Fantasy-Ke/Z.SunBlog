using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yitter.IdGenerator;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Authorization;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.RedisModule;
using Z.SunBlog.Core.CustomConfigModule;
using Z.Fantasy.Core.Entities.Enum;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.UserModule.Dto;
using Lazy.Captcha.Core;
using Z.Fantasy.Core.Attributes;
using Z.SunBlog.Core.UserModule.DomainManager;
using Z.Fantasy.Core.Extensions;
using Z.SunBlog.Application.ConfigModule;
using Z.Fantasy.Core.Authorization.Dtos;
using Z.Fantasy.Core.UserSession;
using Z.Fantasy.Core.Helper;

namespace Z.SunBlog.Application.AuthModule
{
    public interface IAuthAppService : IApplicationService, ITransientDependency
    {
        Task<ZFantasyToken> SignIn(ZUserInfoDto dto);

        IActionResult Captcha([FromQuery] string id);

        Task<ZFantasyToken> RefreshToken(ZFantasyToken token);

        Task ZSignOut([FromBody] string token);
    }
    /// <summary>
    /// 第三方登陆
    /// </summary>
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly ICustomConfigAppService _customConfigService;
        private readonly IIdGenerator _idGenerator;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly ICacheManager _cacheManager;
        private readonly ICaptcha _captcha;
        private readonly IUserDomainManager _userDomainManager;
        public AuthAppService(
            IServiceProvider serviceProvider,
            ICustomConfigAppService customConfigService,
            IIdGenerator idGenerator, IJwtTokenProvider jwtTokenProvider,
            IHttpContextAccessor httpContextAccessor,
            ICacheManager cacheManager, ICaptcha captcha, IUserDomainManager userDomainManager) : base(serviceProvider)
        {
            _customConfigService = customConfigService;
            _idGenerator = idGenerator;
            _jwtTokenProvider = jwtTokenProvider;
            _cacheManager = cacheManager;
            _captcha = captcha;
            _userDomainManager = userDomainManager;
        }

        /// <summary>
        /// 系统用户登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ZFantasyToken> SignIn(ZUserInfoDto dto)
        {
            bool validate = _captcha.Validate(dto.Id, dto.Code);
            if (!validate)
            {
                throw new UserFriendlyException("验证码错误");
            }

            string signInErrorCacheKey = $"login.error.{dto.UserName}";
            var value = await _cacheManager.GetCacheAsync<int>(signInErrorCacheKey);
            var setting = await _customConfigService.Get<SysSecuritySetting>();
            //5分钟内连续验证密码失败超过4次将限制用户尝试
            if (value > (setting?.Times ?? 4))
            {
                throw new UserFriendlyException("由于您多次登录失败，系统已限制账户登录");
            }
            var user = await _userDomainManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.UserName == dto.UserName);
            if (user == null)
            {
                throw new UserFriendlyException("用户名或密码错误");
            }

            if (user.Status == AvailabilityStatus.Disable || user.LockExpired.HasValue && DateTime.Now < user.LockExpired)
            {
                throw new UserFriendlyException("您的账号被锁定");
            }

            if (user.PassWord != MD5Encryption.Encrypt($"{_idGenerator.Encode(user.Id)}{dto.PassWord}"))
            {
                await _cacheManager.SetCacheAsync(signInErrorCacheKey, value + 1, TimeSpan.FromMinutes(5));
                throw new UserFriendlyException("用户名或密码错误");
            }
            var tokenConfig = AppSettings.AppOption<JwtSettings>("App:JWtSetting");
            // 设置Token的Claims
            List<Claim> claims = new List<Claim>
            {
               new Claim(ZClaimTypes.UserName, user.Name!), //HttpContext.User.Identity.Name
                new Claim(ZClaimTypes.UserId, user.Id!.ToString()),
                new Claim(ZClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(tokenConfig.AccessTokenExpirationMinutes).ToString()),
            };
            var token = _jwtTokenProvider.GenerateZToken(claims.ToArray());

            await _cacheManager.SetCacheAsync($"Token_{user.Id}", new ZFantasyToken { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken }, TimeSpan.FromMilliseconds(tokenConfig.AccessTokenExpirationMinutes));
            await _cacheManager.SetCacheAsync($"Token_{user.Id}_Refresh", new ZFantasyToken { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken }, TimeSpan.FromMilliseconds(tokenConfig.AccessTokenExpirationMinutes));
            return token;
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ZFantasyToken> RefreshToken(ZFantasyToken token)
        {
            var principal = _jwtTokenProvider.GetPrincipalToken(token.AccessToken);
            if (principal?.Claims?.Any() ?? false)
            {
                string id = principal?.Claims?.FirstOrDefault(m => m.Type == ZClaimTypes.UserId)?.Value.CastTo<string>() ?? "";

                var zToken = await _cacheManager.GetCacheAsync<ZFantasyToken>($"Token_{id}_Refresh");

                if (zToken == null || zToken.RefreshToken != token.RefreshToken)
                {
                    return null;
                }
                var tokenConfig = AppSettings.AppOption<JwtSettings>("App:JWtSetting");
                DateTime expireTime = DateTime.Now.AddSeconds(tokenConfig.AccessTokenExpirationMinutes);
                var list = principal.Claims.Where(m => m.Type != ClaimTypes.Expiration).ToList();
                list.Add(new Claim(ClaimTypes.Expiration, expireTime.ToString()));
                var newToken = _jwtTokenProvider.GenerateZToken(list.ToArray());
                int expireMinutes = tokenConfig.RefreshTokenExpirationMinutes;
                await _cacheManager.SetCacheAsync($"Token_{id}_Refresh", newToken, TimeSpan.FromMinutes(expireMinutes));
                return newToken;
            }
            return null;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ZSignOut([FromBody] string token)
        {
            if (token.IsNullWhiteSpace()) throw new UserFriendlyException("token不存在!");
            var userid = UserService.UserId;
            if (!userid.IsNullWhiteSpace())
            {
                await _cacheManager.RemoveCacheAsync($"Token_{userid}");
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="id">验证码唯一id</param>
        /// <returns></returns>
        [HttpGet]
        [NoResult]
        public IActionResult Captcha([FromQuery] string id)
        {
            var data = _captcha.Generate(id);
            var stream = new MemoryStream(data.Bytes);
            return new FileStreamResult(stream, "image/gif");
        }

    }
}
