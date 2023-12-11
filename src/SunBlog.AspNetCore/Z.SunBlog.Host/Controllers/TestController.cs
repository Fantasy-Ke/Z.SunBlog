using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Z.Fantasy.Core.Authorization;
using Z.Fantasy.Core.UserSession;
using Z.SunBlog.Application.UserModule;
using Serilog;
using Z.Fantasy.Core.RedisModule;
using Z.SunBlog.Application.UserModule.Dto;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Helper;
using Z.EventBus.EventBus;
using Z.SunBlog.Core.Handlers.TestHandlers;
using Z.Fantasy.Core.AutofacExtensions;
using Z.SunBlog.Application.AlbumsModule.BlogServer;
using Z.SunBlog.Application.MenuModule;

namespace Z.SunBlog.Host.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserSession _userSession;
        private readonly IUserAppService _userAppService;
        private ICacheManager _cacheManager;
        private readonly ILocalEventBus _localEvent;
        public TestController(IJwtTokenProvider jwtTokenProvider, IUserSession userSession, IUserAppService userAppService, ICacheManager cacheManager, ILocalEventBus localEvent)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userSession = userSession;
            _userAppService = userAppService;
            _cacheManager = cacheManager;
            _localEvent = localEvent;
        }





        /// <summary>
        /// 获取jwttoken
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Login(ZUserInfoDto user)
        {
            var userinfo = await _userAppService.Login(user);
            if (userinfo == null)
            {
                throw new UserFriendlyException("账号密码错误");

            }
            var tokenConfig = AppSettings.AppOption<JwtSettings>("App:JWtSetting");
            // 设置Token的Claims
            List<Claim> claims = new List<Claim>
            {
               new Claim(ZClaimTypes.UserName, userinfo.UserName!), //HttpContext.User.Identity.Name
                new Claim(ZClaimTypes.UserId, userinfo.Id!.ToString()),
                new Claim(ZClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(tokenConfig.AccessTokenExpirationMinutes).ToString()),
            };
            var token = _jwtTokenProvider.GenerateZToken(claims.ToArray());

            Response.Cookies.Append("access-token", token.AccessToken, new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(tokenConfig.AccessTokenExpirationMinutes)
             });

            Log.Logger.Information("登录成功");
            return token.AccessToken;
        }


        /// <summary>
        /// 获取jwttoken
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ZAuthorization]
        public async Task<string> GetUser()
        {
           var ser =  IOCManager.GetService<IMenuAppService>();
            var list =  await ser.TreeSelect();
            var user = _userSession.UserName;
            var userid = _userSession.UserId;

            return user;
        }

        [HttpGet]
        public async Task<string> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return "ddd";
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ZAuthorization]
        public async Task<JsonResult> CreateUser()
        {
            return new JsonResult(await _userAppService.Create());
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ZAuthorization]
        public async Task<List<ZUserInfoDto>> SeacthUser()
        {

            var user = await _userAppService.GetFrist();

            await _cacheManager.SetCacheAsync("user1", user);

            return user;
        }


        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ZAuthorization]
        public async Task<List<ZUserInfoDto>> SeacthUserCache()
        {

            var user = await _cacheManager.GetCacheAsync<List<ZUserInfoDto>>("user1");

            return user;
        }


        /// <summary>
        /// 同步消费事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventCache()
        {
            for (var i = 0; i < 100; i++)
            {
                TestDto eto = new TestDto()
                {
                    Name = "LocalEventBus" + i.ToString(),
                    Description = "zzzz" + i.ToString(),
                };
                await _localEvent.PushAsync(eto);
            }

            Log.Warning("我什么时候开始得");
        }

        /// <summary>
        /// 后台消费事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventChnnalCache()
        {
            for (var i = 0; i < 100; i++)
            {
                TestDto eto = new TestDto()
                {
                    Name = "LocalEventBus" + i.ToString(),
                    Description = "zzzz" + i.ToString(),
                };
                await _localEvent.EnqueueAsync(eto);
            }

            Log.Warning("我什么时候开始得");
        }

    }
}
