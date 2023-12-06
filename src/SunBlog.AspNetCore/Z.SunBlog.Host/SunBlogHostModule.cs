using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MrHuo.OAuth;
using MrHuo.OAuth.QQ;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Yitter.IdGenerator;
using Z.Ddd.Application.Middleware;
using Z.Ddd.Common;
using Z.Ddd.Common.Extensions;
using Z.Ddd.Common.Serilog.Utility;
using Z.EntityFrameworkCore.Extensions;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;
using Z.SunBlog.Application;
using Z.SunBlog.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio.DataModel;
using MrHuo.OAuth.Gitee;
using Microsoft.Extensions.DependencyInjection;
using MrHuo.OAuth.Github;
using Z.Ddd.Common.Helper;
using Z.Ddd.Common.DynamicWebAPI;
using Z.EventBus.Extensions;

namespace Z.SunBlog.Host;

[DependOn(typeof(SunBlogApplicationModule),
    typeof(SunBlogEntityFrameworkCoreModule))]
public class SunBlogHostModule : ZModule
{
    protected IHostEnvironment env { get; private set; }
    protected IConfiguration configuration { get; private set; }

    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigerContext context)
    {

        configuration = context.GetConfiguration();
        env = context.Environment();
        context.Services
                .AddMvc()
                .AddRazorPagesOptions(options => { })
                .AddRazorRuntimeCompilation()
                .AddDynamicWebApi();

        ServicesJwtToken(context.Services);

        ServicesSwagger(context.Services);

        context.Services.AddCors(
            options => options.AddPolicy(
                name: "ZCores",
                builder => builder.AllowAnyHeader()
                    .AllowAnyMethod().AllowCredentials().WithOrigins(
                    configuration["App:CorsOrigins"]!
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)//获取移除空白字符串
                    .Select(o => o.RemoveFix("/"))
                    .ToArray()
                    )
            ));

        // 注入自动事务中间件
        context.Services.AddUnitOfWorkMiddleware();

        //雪花id 文档：https://github.com/yitter/IdGenerator
        context.Services.AddIdGenerator(AppSettings.AppOption<IdGeneratorOptions>("SnowId"));

        context.Services.AddSingleton(new GiteeOAuth(OAuthConfig.LoadFrom(configuration, "oauth:gitee")));
        context.Services.AddSingleton(new QQOAuth(OAuthConfig.LoadFrom(configuration, "oauth:qq")));
        context.Services.AddSingleton(new GithubOAuth(OAuthConfig.LoadFrom(configuration, "oauth:github")));

        #region 图形验证码

        //图形验证码
        context.Services.AddCaptcha(configuration, option =>
        {
            option.CaptchaType = CaptchaType.WORD_NUMBER_LOWER; // 验证码类型
            option.CodeLength = 4; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
            option.ExpirySeconds = 60; // 验证码过期时间
            option.IgnoreCase = true; // 比较时是否忽略大小写
            option.StoreageKeyPrefix = ""; // 存储键前缀

            option.ImageOption.Animation = true; // 是否启用动画
            option.ImageOption.FrameDelay = 300; // 每帧延迟,Animation=true时有效, 默认30

            option.ImageOption.Width = 132; // 验证码宽度
            option.ImageOption.Height = 40; // 验证码高度
            //option.ImageOption.BackgroundColor = SixLabors.ImageSharp.Color.White; // 验证码背景色

            option.ImageOption.BubbleCount = 2; // 气泡数量
            option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
            option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
            option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

            option.ImageOption.InterferenceLineCount = 2; // 干扰线数量

            option.ImageOption.FontSize = 36; // 字体大小
            option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体

            /* 
             * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
             * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
             */

            option.ImageOption.TextBold = true;// 粗体，该配置2.0.3新增
        });

        #endregion
    }

    /// <summary>
    /// 初始化应用
    /// </summary>
    /// <param name="context"></param>
    public override void OnInitApplication(InitApplicationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseCors("ZCores");

        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = SerilogRequestUtility.HttpMessageTemplate;
            options.GetLevel = SerilogRequestUtility.GetRequestLevel;
            options.EnrichDiagnosticContext = SerilogRequestUtility.EnrichFromRequest;
        });

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseRouting();
        //鉴权中间件
        app.UseAuthentication();

        app.UseStaticFiles();

        app.UseAuthorization();

        app.UseUnitOfWorkMiddleware();

        UseSwagger(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });
    }

    #region ConfigureServices
    /// <summary>
    /// Swagger
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ServicesSwagger(IServiceCollection services)
    {


        services.AddSwaggerGen(options =>
        {
            //过滤器
            options.OperationFilter<AddResponseHeadersFilter>();
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "SunBlog API v1",
                Title = "SunBlog API",
                Description = "Web API for managing By Z.SunBlog",
                TermsOfService = new Uri("https://github.com/Fantasy-Ke"),
                Contact = new OpenApiContact
                {
                    Name = "github 地址",
                    Url = new Uri("https://github.com/Fantasy-Ke/Z.SunBlog")
                },
                License = new OpenApiLicense
                {
                    Name = "个人博客",
                    Url = new Uri("https://www.cnblogs.com/fantasy-ke/")
                }
            });
            options.OrderActionsBy(o => o.RelativePath);

            var xmlList = Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList();
            xmlList.ForEach(xml => options.IncludeXmlComments(xml));
        });
    }


    /// <summary>
    /// jwt服务
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ServicesJwtToken(IServiceCollection services)
    {


        var config = configuration.GetSection("App:JWtSetting").Get<JwtSettings>(); // 从appsettings.json读取JwtConfig配置
        // 添加JWT身份验证服务
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddCookie(options =>
        {
            //cokkie名称
            options.Cookie.Name = "Z.BearerCokkie";
            //cokkie过期时间
            options.ExpireTimeSpan = TimeSpan.FromMinutes(config!.CokkieExpirationMinutes);
            //cokkie启用滑动过期时间
            options.SlidingExpiration = false;

            options.LogoutPath = "/Home/Index";

            options.Events = new CookieAuthenticationEvents
            {
                OnSigningOut = async context =>
                {
                    context.Response.Cookies.Delete("access-token");

                    await Task.CompletedTask;
                }
            };
        })
        .AddJwtBearer(options =>
        {

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true, //是否验证Issuer
                ValidIssuer = config!.Issuer, //发行人Issuer
                ValidateAudience = true, //是否验证Audience
                ValidAudience = config.Audience,//
                ValidateIssuerSigningKey = true, //是否验证SecurityKey
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey)), //SecurityKey
                ValidateLifetime = true, //是否验证失效时间
                ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                RequireExpirationTime = true,
                SaveSigninToken = true,
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies["access-token"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },
            };
        });
    }
    #endregion

    #region InitApp

    /// <summary>
    /// swagger
    /// </summary>
    /// <param name="app"></param>
    protected virtual void UseSwagger(IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "SunBlog API V1");
            options.EnableDeepLinking();//深链接功能
            options.DocExpansion(DocExpansion.None);//swagger文档是否打开
            options.IndexStream = () =>
            {
                var path = Path.Join(env.ContentRootPath, "wwwroot", "pages", "swagger.html");
                return new FileInfo(path).OpenRead();
            };
        });
    }

    #endregion



}
