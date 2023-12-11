
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.Json;
using Z.Fantasy.Core.Helper;

namespace Z.SunBlog.EntityFrameworkCore.ConfigureExtensions;

public static class BlogEntityFrameworkCoreExtensions
{
    public static ModelBuilder ConfigureModel(this ModelBuilder builder)
    {
        //builder.Entity<UserInfo>(x =>
        //{
        //    x.ToTable("UserInfos");

        //    x.AddSimpleConfigure();

        //});
        var dbType = AppSettings.AppOption<string>("App:DbType");
        switch (dbType.ToLower())
        {
            case "mysql":
                builder.UseCollation("utf8mb4_general_ci");
                break;
            case "sqlserver":
                builder.UseCollation("Chinese_PRC_CI_AS");
                break;
            default:
                break;
        }

        return builder;
    }

}