using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Z.Ddd.Common.Exceptions;
using Z.Ddd.Common.Helper;
using Z.Ddd.Common.UnitOfWork;
using Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed;

public static class SeedHelper
{
    public static void SeedDbData(SunBlogDbContext dbContext, IServiceProvider serviceProvider)
    {
        var dbtype = AppSettings.AppOption<string>("App:DbType")!;
        switch (dbtype.ToLower())
        {
            case "sqlserver":
                var isConnect = dbContext.Database.CanConnect();
                if (!isConnect) throw new UserFriendlyException($"数据库连接错误,连接字符串:'{dbContext.Database.GetConnectionString()}'");
                break;
            case "mysql":
                break;
            default:
                throw new UserFriendlyException("不支持的数据库类型");
        }
        
        WithDbContext(serviceProvider, dbContext, SeedDbData);
    }

    public static void SeedDbData(SunBlogDbContext context)
    {
        new DefaultUserBuilder(context).Create();
        new DefaultRoleBuilder(context).Create();
        new DefaultOrganizationBuilder(context).Create();
        new DefaultMenuBuilder(context).Create();
        new DefaultCustomconfigBuilde(context).Create();
        new DefaultCustomconfigitemBuilde(context).Create();
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
