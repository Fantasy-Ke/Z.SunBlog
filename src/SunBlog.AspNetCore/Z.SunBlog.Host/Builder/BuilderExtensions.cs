﻿using Lazy.Captcha.Core.Generator;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Z.Fantasy.Core;
using Z.Fantasy.Core.DynamicWebAPI;
using Z.Fantasy.Core.Extensions;
using Z.Module;
using Z.Module.Extensions;
using Z.EntityFrameworkCore.Extensions;
using Serilog;
using Z.Fantasy.Application.Middleware;
using Z.Fantasy.Core.Serilog.Utility;
using System.Data;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.Entities.Enum;
using Hangfire.MySql;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Hangfire;
using StackExchange.Redis;
using Z.Fantasy.Core.Exceptions;

namespace Z.SunBlog.Host.Builder
{
    public static class BuilderExtensions
    {
        public static IHostEnvironment Env { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 基础信息注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.ServicesConfig();
            services.ConfigureHangfireService();
            services.ServicesMvc();
            services.ServicesSwagger();
            services.ServicesCors();
            // 注入自动事务中间件
            services.AddUnitOfWorkMiddleware();
            services.ServicesJwtToken();
            services.ServicesCaptcha();
        }

        #region ConfigureServices
        /// <summary>
        /// 注入基础
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesConfig(this IServiceCollection services)
        {
            var context = new ServiceConfigerContext(services);
            Configuration = context.GetConfiguration();
            Env = context.Environment();
            ZConfigBase.DatabaseType =  AppSettings.AppOption<DatabaseType>("App:DbType")!;
        }

        

        /// <summary>
        /// mvc动态api配置
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesMvc(this IServiceCollection services)
        {
            services
               .AddMvc()
               .AddRazorPagesOptions(options => { })
               .AddRazorRuntimeCompilation()
               .AddDynamicWebApi();
        }

        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesSwagger(this IServiceCollection services)
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
        /// 跨域
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesCors(this IServiceCollection services)
        {
            services.AddCors(
            options => options.AddPolicy(
                name: "ZCores",
                builder => builder.AllowAnyHeader()
                    .AllowAnyMethod().AllowCredentials().WithOrigins(
                    Configuration["App:CorsOrigins"]!
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)//获取移除空白字符串
                    .Select(o => o.RemoveFix("/"))
                    .ToArray()
                    )
            ));
        }

        /// <summary>
        /// jwt服务
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesJwtToken(this IServiceCollection services)
        {


            var config = Configuration.GetSection("App:JWtSetting").Get<JwtSettings>(); // 从appsettings.json读取JwtConfig配置
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

                        var signlRToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(signlRToken) && (path.StartsWithSegments("/api/chatHub")))
                        {
                            context.Token = signlRToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });
        }

        /// <summary>
        /// 注册Hangfire
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureHangfireService(this IServiceCollection services)
        {
            var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireEnabled");
            if (!enable) return;
            services.AddHangfire(config =>config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)//向前兼容
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseZHangfireStorage()
                .UseSerilogLogProvider()
            );

            services.AddHangfireServer(optionsAction: c =>
            {
                //wait all jobs performed when BackgroundJobServer shutdown.
                c.ShutdownTimeout = TimeSpan.FromMinutes(30);
                c.Queues = new[] { "default", "jobs" }; //队列名称，只能为小写
                c.WorkerCount = 3; //Environment.ProcessorCount * 5, //并发任务数 Math.Max(Environment.ProcessorCount, 20)
                c.ServerName = "fantasy.hangfire";
            });
        }


        /// <summary>
        /// 使用 Hangfire Storage
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseZHangfireStorage(this IGlobalConfiguration configuration)
        {
            string connectionString = string.Empty;
            var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireRedis");
            if (enable)
            {
                var redisSrting = AppSettings.AppOption<string>("App:RedisCache:Configuration");
                var redisOptions = ConfigurationOptions.Parse(redisSrting);
                configuration.UseRedisStorage(ConnectionMultiplexer.Connect(redisOptions), new RedisStorageOptions
                {
                    InvisibilityTimeout = TimeSpan.FromMinutes(30.0),
                    FetchTimeout = TimeSpan.FromMinutes(3.0),
                    ExpiryCheckInterval = TimeSpan.FromHours(1.0),
                    Db = 1,
                    Prefix = "Z_Fantasy:",
                    SucceededListSize = 499,
                    DeletedListSize = 499,
                    LifoQueues = new string[0],
                    UseTransactions = true,
                });
                goto redis;
            }
            switch (ZConfigBase.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    connectionString = AppSettings.AppOption<string>("App:ConnectionString:SqlServer");
                    configuration.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true,
                        SchemaName = "Z_HangFire_"
                    });
                    break;
                case DatabaseType.MySql:
                    connectionString = AppSettings.AppOption<string>("App:ConnectionString:Mysql");
                    configuration.UseMysqlStorage(connectionString);
                    break;
                default:
                    throw new UserFriendlyException("不支持的数据库类型");
            }

        redis:
            return configuration;
        }

        /// <summary>
        /// 使用Oracle的Hangfire Storage
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseMysqlStorage(this IGlobalConfiguration configuration, string connectionString)
        {

            var storage = new MySqlStorage(connectionString, new MySqlStorageOptions()
            {
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Z_HangFire_"
            });

            configuration.UseStorage(storage);

            return configuration;
        }


        /// <summary>
        /// 图形验证码
        /// </summary>
        /// <param name="services"></param>
        public static void ServicesCaptcha(this IServiceCollection services)
        {
            //图形验证码
            services.AddCaptcha(Configuration, option =>
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

        }

        #endregion

        /// <summary>
        /// 管道注入
        /// </summary>
        /// <param name="app"></param>
        public static void AddUseCore(this IApplicationBuilder app)
        {
            app.UseCors("ZCores");

            app.UseStaticFiles();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = SerilogRequestUtility.HttpMessageTemplate;
                options.GetLevel = SerilogRequestUtility.GetRequestLevel;
                options.EnrichDiagnosticContext = SerilogRequestUtility.EnrichFromRequest;
            });

            //中间件
            app.UseMiddleware();

            //鉴权中间件
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfire();

            app.UseRouting();

            app.UseAddSwagger();


        }

        #region InitApp

        /// <summary>
        /// 启用Hangfire
        /// </summary>
        /// <param name="app"></param>
        public static void UseHangfire(this IApplicationBuilder app)
        {
            // TODO: 判断是否启用 HangfireDashboard
            //配置服务最大重试次数值
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute 
            { 
                Attempts = 5, 
                OnAttemptsExceeded = AttemptsExceededAction.Fail 
            });
            var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireDashboardEnabled");
            if (!enable) return;
            //启用Hangfire仪表盘和服务器(支持使用Hangfire而不是默认的后台作业管理器)
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DefaultRecordsPerPage = 10,
                DarkModeEnabled = true,
                DashboardTitle = "Fantasy_ke Hangfire",
            });

        }

        /// <summary>
        /// 中间件管道注入
        /// </summary>
        /// <param name="app"></param>
        public static void UseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLogMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseUnitOfWorkMiddleware();
        }

        /// <summary>
        /// swagger管道
        /// </summary>
        /// <param name="app"></param>
        public static void UseAddSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SunBlog API V1");
                options.EnableDeepLinking();//深链接功能
                options.DocExpansion(DocExpansion.None);//swagger文档是否打开
                options.IndexStream = () =>
                {
                    var path = Path.Join(Env.ContentRootPath, "wwwroot", "pages", "swagger.html");
                    return new FileInfo(path).OpenRead();
                };
            });
        }

        #endregion
    }
}
