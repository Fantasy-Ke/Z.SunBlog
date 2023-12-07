using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Linq;
using Z.Ddd.Application;
using Z.Ddd.Common.Authorization.Authorize;
using Z.Ddd.Common.AutoMapper;
using Z.EventBus.Extensions;
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

namespace Z.SunBlog.Application
{
    [DependOn(typeof(SunBlogCoreModule), typeof(ZDddApplicationModule))]
    public class SunBlogApplicationModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            ConfigureAutoMapper();
            AuthorizeRegister.Register.Init(context.Services);
        }

        public override void OnInitApplication(InitApplicationContext context)
        {

        }

        public override void PostInitApplication(InitApplicationContext context)
        {
            var scope = context.ServiceProvider.CreateScope();
            var authorizeManager = scope.ServiceProvider.GetRequiredService<IAuthorizeManager>();

            IAuthorizePermissionContext permissionContext = new AuthorizePermissionContext();
            //authorizeManager.AddAuthorizeRegiester(permissionContext).Wait();
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