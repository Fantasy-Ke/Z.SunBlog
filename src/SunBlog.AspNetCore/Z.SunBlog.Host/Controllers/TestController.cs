using System.Security.Claims;
using System.Xml.Linq;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Z.EventBus.EventBus;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Authorization;
using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.RedisModule;
using Z.Fantasy.Core.UserSession;
using Z.RabbitMQ.Manager;
using Z.SunBlog.Application.AlbumsModule.BlogServer;
using Z.SunBlog.Application.CommentsModule.Channel;
using Z.SunBlog.Application.HangfireJob.RequestLog;
using Z.SunBlog.Application.MenuModule;
using Z.SunBlog.Application.UserModule;
using Z.SunBlog.Application.UserModule.Dto;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.Handlers.TestHandlers;
using Z.SunBlog.Core.jobs.TestJob;

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
        private readonly IRabbitEventManager _rabbitEventManager;

        private readonly IBackgroundJobManager backgroundJobManager;

        public TestController(
            IJwtTokenProvider jwtTokenProvider,
            IUserSession userSession,
            IUserAppService userAppService,
            ICacheManager cacheManager,
            ILocalEventBus localEvent,
            IBackgroundJobManager backgroundJobManager,
            IRabbitEventManager rabbitEventManager
        )
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userSession = userSession;
            _userAppService = userAppService;
            _cacheManager = cacheManager;
            _localEvent = localEvent;
            this.backgroundJobManager = backgroundJobManager;
            _rabbitEventManager = rabbitEventManager;
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
                new Claim(
                    ZClaimTypes.Expiration,
                    DateTimeOffset
                        .Now.AddMinutes(tokenConfig.AccessTokenExpirationMinutes)
                        .ToString()
                ),
            };
            var token = _jwtTokenProvider.GenerateZToken(claims.ToArray());

            Response.Cookies.Append(
                "access-token",
                token.AccessToken,
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(
                        tokenConfig.AccessTokenExpirationMinutes
                    )
                }
            );

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
            var ser = IOCManager.GetService<IMenuAppService>();
            var list = await ser.TreeSelect();
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

        /// <summary>
        /// 自己加的hangfire
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task TestJobs1()
        {
            await backgroundJobManager.EnqueueAsync(new TestJobDto() { Id = Guid.NewGuid() });
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task TestJobs2()
        {
            await backgroundJobManager.EnqueueAsync(
                new TestJobDto() { Id = Guid.NewGuid() },
                TimeSpan.FromSeconds(10)
            );
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task TestJobs3()
        {
            await backgroundJobManager.AddOrUpdateScheduleAsync(new RequestLogJob());
        }

        /// <summary>
        /// 消费rabbit队列
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task RabbitSubscribe()
        {
           await  _rabbitEventManager.SubscribeAsync<CommentsConsumer>("comment");
        }

        /// <summary>
        /// 推送rabbit队列
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task RabbitPublish()
        {
           await  _rabbitEventManager.PublishAsync<CommentsConsumer, Comments>(
                "comment",
                new Comments()
                {
                    //"moduleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    // "rootId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    // "parentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    // "replyAccountId": "string",
                    // "content": "fdsafd发顺丰"
                    ModuleId = Guid.NewGuid(),
                    RootId = Guid.NewGuid(),
                    ParentId = Guid.NewGuid(),
                    ReplyAccountId = Guid.NewGuid().ToString("N"),
                    Content = $"测试消息队列 Guid：{Guid.NewGuid()}"
                }
            );
        }

        /// <summary>
        /// 消费死信队列
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task RabbitSubscribeDLXAsync()
        {
           await  _rabbitEventManager.SubscribeDLXAsync<CommentsConsumer>("comment");
        }


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task UnSubscribe()
        {
           await  _rabbitEventManager.UnSubscribeAsync<CommentsConsumer>("comment");
        }
    }
}
