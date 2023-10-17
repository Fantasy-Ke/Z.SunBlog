﻿using Microsoft.Extensions.DependencyInjection;
using Z.EntityFrameworkCore;
using Z.EntityFrameworkCore.Core;
using Z.EntityFrameworkCore.SqlServer;
using Z.EntityFrameworkCore.SqlServer.Extensions;
using Z.Module;
using Z.Module.Modules;
using Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed;

namespace Z.SunBlog.EntityFrameworkCore
{
    [DependOn(typeof(ZSqlServerEntityFrameworkCoreModule))]
    public class SunBlogEntityFrameworkCoreModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            context.AddSqlServerEfCoreEntityFrameworkCore<SunBlogDbContext>();
            context.UseRepository<SunBlogDbContext>();
        }

        public override void PostInitApplication(InitApplicationContext context)
        {
            var entityManager = context.ServiceProvider
                 .GetRequiredService<IEntityManager<SunBlogDbContext>>();

            //添加种子数据
            entityManager.DbSeed((dbcontext) =>
            {
                SeedHelper.SeedDbData(dbcontext, context.ServiceProvider);
            });
        }
    }
}