﻿using Microsoft.Extensions.DependencyInjection;
using Z.EntityFrameworkCore;
using Z.EntityFrameworkCore.Core;
using Z.EntityFrameworkCore.SqlServer;
using Z.EntityFrameworkCore.SqlServer.Extensions;
using Z.EntityFrameworkCore.Mysql.Extensions;
using Z.Module;
using Z.Module.Extensions;
using Z.Module.Modules;
using Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed;
using Microsoft.Extensions.Configuration;
using Z.EntityFrameworkCore.Mysql;
using Z.Foundation.Core.Entities.Enum;
using Z.Foundation.Core;
using Z.Foundation.Core.Exceptions;

namespace Z.SunBlog.EntityFrameworkCore
{
    [DependOn(typeof(ZSqlServerEntityFrameworkCoreModule),
    typeof(ZMysqlEntityFrameworkCoreModule))]
    public class SunBlogEntityFrameworkCoreModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            var configuration = context.GetConfiguration();
            string? connectionString = string.Empty;
            switch (ZConfigBase.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    connectionString = configuration.GetSection("App:ConnectionString:SqlServer").Get<string>();
                    context.AddSqlServerEfCoreEntityFrameworkCore<SunBlogDbContext>(connectionString);
                    break;
                case DatabaseType.MySql:
                    connectionString = configuration.GetSection("App:ConnectionString:Mysql").Get<string>();
                    context.AddMysqlEfCoreEntityFrameworkCore<SunBlogDbContext>(new Version(8, 0,21), connectionString);
                    break;
                default:
                    throw new UserFriendlyException("不支持的数据库类型");
            }
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