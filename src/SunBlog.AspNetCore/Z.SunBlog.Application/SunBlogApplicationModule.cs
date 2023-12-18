using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Application;
using Z.Fantasy.Core.Authorization.Authorize;
using Z.Fantasy.Core.AutoMapper;
using Z.Module;
using Z.Module.Modules;
using Z.SunBlog.Application.AlbumsModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.ArticleModule.BlogClient.MapperConfig;
using Z.SunBlog.Application.ArticleModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.CategoryModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.CommentsModule.BlogClient.MapperConfig;
using Z.SunBlog.Application.ConfigModule.MapperConfig;
using Z.SunBlog.Application.FriendLinkModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.MenuModule.MapperConfig;
using Z.SunBlog.Application.OAuthModule.MapperConfig;
using Z.SunBlog.Application.PictureModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.SystemServiceModule.OrganizationService.MapperConfig;
using Z.SunBlog.Application.SystemServiceModule.RoleService.MapperConfig;
using Z.SunBlog.Application.SystemServiceModule.UserService.MapperConfig;
using Z.SunBlog.Application.TagsModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.TalksModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.UserModule.MapperConfig;
using Z.SunBlog.Core;
using Z.EventBus.Extensions;
using Z.Fantasy.Application.Handlers;
using Z.SunBlog.Core.Handlers.FileHandlers;
using Z.SunBlog.Core.Handlers.TestHandlers;

namespace Z.SunBlog.Application
{
    /// <summary>
    /// SunBlog应用层
    /// </summary>
    [DependOn(typeof(SunBlogCoreModule), typeof(ZFantasyApplicationModule))]
    public class SunBlogApplicationModule : ZModule
    {
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            ConfigureAutoMapper();

            //事件Handler
            context.Services.EventBusSubscribes(c =>
            {
                c.Subscribe<FileEventDto, FileEventHandler>();
                c.Subscribe<TestDto, TestEventHandler>();
                c.Subscribe<RequestLogDto, RequestLogEventHandler>();

            });
            AuthorizeRegister.Register.Init(context.Services);
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task PostInitApplicationAsync(InitApplicationContext context)
        {
            var scope = context.ServiceProvider.CreateAsyncScope();
            var authorizeManager = scope.ServiceProvider.GetRequiredService<IAuthorizeManager>();
            IAuthorizePermissionContext permissionContext = new AuthorizePermissionContext();

            await authorizeManager.AddAuthorizeRegiester(permissionContext);
        }

        private void PermissionProvider()
        {

        }

        private void ConfigureAutoMapper()
        {
            Configure<ZAutoMapperOptions>(options =>
            {
                options.Configurators.Add(ctx =>
                {
                    UserAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    ArticleSAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    ArticleCAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    AlbumsAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    TagsAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    TalksAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    CommentsCAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    FriendLinkAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    OAuthAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    UserSysMapper.CreateMappings(ctx.MapperConfiguration);
                    RoleSysMapper.CreateMappings(ctx.MapperConfiguration);
                    OrganizationMapper.CreateMappings(ctx.MapperConfiguration);
                    MenuAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    PictureAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    CategoryAutoMapper.CreateMappings(ctx.MapperConfiguration);
                    CustomConfigAutoMapper.CreateMappings(ctx.MapperConfiguration);

                });
            });
        }
    }
}