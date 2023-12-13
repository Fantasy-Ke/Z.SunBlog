using MrHuo.OAuth;
using MrHuo.OAuth.QQ;
using Yitter.IdGenerator;
using Z.Fantasy.Core.Extensions;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;
using Z.SunBlog.Application;
using Z.SunBlog.EntityFrameworkCore;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.OAuth.Gitee;
using Z.Fantasy.Core.OAuth.GitHub;
using Z.Fantasy.Core.Hubs;

namespace Z.SunBlog.Host;

/// <summary>
/// SunBlogHostModule
/// </summary>
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

        //雪花id 文档：https://github.com/yitter/IdGenerator
        context.Services.AddIdGenerator(AppSettings.AppOption<IdGeneratorOptions>("SnowId"));

        context.Services.AddSingleton(new ZGiteeOAuth(OAuthConfig.LoadFrom(configuration, "oauth:gitee")));
        context.Services.AddSingleton(new QQOAuth(OAuthConfig.LoadFrom(configuration, "oauth:qq")));
        context.Services.AddSingleton(new ZGitHubOAuth(OAuthConfig.LoadFrom(configuration, "oauth:github")));

        //ioc储存
        AppSettings.ConfigurationProvider(context.Services);
    }

    /// <summary>
    /// 初始化应用
    /// </summary>
    /// <param name="context"></param>
    public override void OnInitApplication(InitApplicationContext context)
    {
        var app = context.GetApplicationBuilder();
        

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
            endpoints.MapHub<ChatHub>("/api/chatHub");
        });
    }
}
