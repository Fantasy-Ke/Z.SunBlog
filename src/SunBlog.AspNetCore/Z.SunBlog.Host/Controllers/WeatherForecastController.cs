using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Authorization;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.UserSession;

namespace Z.SunBlog.Host.Controllers
{

    /// <summary>
    /// 天气控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="logger"></param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IJwtTokenProvider jwtTokenProvider)
        {
            _logger = logger;
            _jwtTokenProvider = jwtTokenProvider;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>��ϸ����</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            UserTokenModel tokenModel = new UserTokenModel();
            tokenModel.UserName = "test";
            tokenModel.UserId =Guid.NewGuid().ToString("N");
            var tokenConfig = AppSettings.AppOption<JwtSettings>("App:JWtSetting");
            // 设置Token的Claims
            List<Claim> claims = new List<Claim>
            {
               new Claim(ZClaimTypes.UserName, tokenModel.UserName!), //HttpContext.User.Identity.Name
                new Claim(ZClaimTypes.UserId, tokenModel.UserId!.ToString()),
                new Claim(ZClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(tokenConfig.AccessTokenExpirationMinutes).ToString()),
            };
            var token = _jwtTokenProvider.GenerateZToken(claims.ToArray());

            return token.AccessToken;
        }
    }
}