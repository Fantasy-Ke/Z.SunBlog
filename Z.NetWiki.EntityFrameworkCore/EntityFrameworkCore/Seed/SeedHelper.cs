﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Z.Ddd.Common.Exceptions;
using Z.Ddd.Common.UnitOfWork;
using Z.EntityFrameworkCore;
using Z.NetWiki.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData;

namespace Z.NetWiki.EntityFrameworkCore.EntityFrameworkCore.Seed;

public static class SeedHelper
{
    public static void SeedDbData(NetWikiDbContext dbContext, IServiceProvider serviceProvider)
    {
        var isConnect = dbContext.Database.CanConnect();
        if (!isConnect) throw new UserFriendlyException($"数据库连接错误,连接字符串:'{dbContext.Database.GetConnectionString()}'");
        WithDbContext(serviceProvider, dbContext, SeedDbData);
    }

    public static void SeedDbData(NetWikiDbContext context)
    {
        new DefaultUserBuilder(context).Create();
    }

    private static void WithDbContext<TDbContext>(IServiceProvider serviceProvider, TDbContext dbContext, Action<TDbContext> contextAction)
            where TDbContext : DbContext
    {

       
        using (var uowManager = serviceProvider.GetRequiredService<IUnitOfWork>())
        {
            using (var uow =  uowManager.BeginTransactionAsync().Result)
            {
                try
                {
                    contextAction(dbContext);

                    uow.CommitAsync();
                }
                catch (Exception ex)
                {
                    uow.RollbackAsync();
                    throw new UserFriendlyException(ex.Message);
                }
                
            }
        }


    }
}
