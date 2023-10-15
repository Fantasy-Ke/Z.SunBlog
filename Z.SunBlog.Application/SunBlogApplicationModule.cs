﻿using Z.Ddd.Application;
using Z.Ddd.Common.AutoMapper;
using Z.Module;
using Z.Module.Modules;
using Z.SunBlog.Application.AlbumsModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.ArticleModule;
using Z.SunBlog.Application.ArticleModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.TagsModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.TalksModule.BlogServer.MapperConfig;
using Z.SunBlog.Application.UserModule.MapperConfig;
using Z.SunBlog.Core;

namespace Z.SunBlog.Application
{
    [DependOn(typeof(SunBlogDomainModule), typeof(ZDddApplicationModule))]
    public class SunBlogApplicationModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            ConfigureAutoMapper();
        }

        public override void OnInitApplication(InitApplicationContext context)
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
                });
            });
        }
    }
}