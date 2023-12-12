using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.UnitOfWork;
using Z.EntityFrameworkCore.Core;
using Z.EntityFrameworkCore.Middlewares;
using Z.EntityFrameworkCore.Options;

namespace Z.EntityFrameworkCore.Extensions;

public static class EfCoreEntityFrameworkCoreExtensions
{
    /// <summary>
    /// 注册efCore基础服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="lifetime"></param>
    /// <typeparam name="TDbContext"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddEfCoreEntityFrameworkCore<TDbContext>(this IServiceCollection services, Action<ZDbContextBuilder>? optionsAction = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TDbContext : ZDbContext<TDbContext>
    {
        // ConfigureOptions(services);
        services.ConfigureDbContext<TDbContext>(optionsAction, lifetime);

        // 注入工作单元 
        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork<TDbContext>));
        services.AddTransient(typeof(IEntityManager<TDbContext>), typeof(EntityManager<TDbContext>));

        return services;
    }

    /// <summary>
    /// 注入SqlServer的DbContext
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="lifetime"></param>
    /// <typeparam name="TDbContext"></typeparam>
    private static IServiceCollection ConfigureDbContext<TDbContext>(this IServiceCollection services, 
        Action<ZDbContextBuilder>? optionsBuilder = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TDbContext : DbContext
    {
        ZDbContextBuilder masaBuilder = new(services, typeof(TDbContext));

        optionsBuilder?.Invoke(masaBuilder);

        services.AddDbContext<TDbContext>(lifetime,lifetime)
            .AddZDbContextOptions<TDbContext>
            (
            (serviceProvider, efDbContextOptionsBuilder) =>
            {
                if (masaBuilder.Builder != null)
                {
                    efDbContextOptionsBuilder.DbContextOptionsBuilder.UseApplicationServiceProvider(serviceProvider);
                    efDbContextOptionsBuilder.DbContextOptionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    masaBuilder.Builder.Invoke(serviceProvider, efDbContextOptionsBuilder.DbContextOptionsBuilder);

                    foreach (var dbContextOptionsBuilder in masaBuilder.DbContextOptionsBuilders)
                    {
                        dbContextOptionsBuilder.Invoke(efDbContextOptionsBuilder.DbContextOptionsBuilder);
                    }
                }
            }
            , masaBuilder.EnableSoftDelete, lifetime);

        return services;
    }

    /// <summary>
    /// 注册ZDbContextOptions
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsBuilder"></param>
    /// <param name="enableSoftDelete"></param>
    /// <param name="optionsLifetime"></param>
    /// <returns></returns>
    private static IServiceCollection AddZDbContextOptions<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, ZDbContextOptionsBuilder>? optionsBuilder,
        bool enableSoftDelete,
        ServiceLifetime optionsLifetime)
        where TDbContext : DbContext
    {
        services.TryAdd(
            new ServiceDescriptor(
                typeof(ZDbContextOptions<TDbContext>),
                serviceProvider => CreateZDbContextOptions<TDbContext>(serviceProvider, optionsBuilder, enableSoftDelete),
                optionsLifetime));

        services.TryAdd(
            new ServiceDescriptor(
                typeof(ZDbContextOptions),
                serviceProvider => serviceProvider.GetRequiredService<ZDbContextOptions<TDbContext>>(),
                optionsLifetime));
        return services;
    }

    private static ZDbContextOptions<TDbContext> CreateZDbContextOptions<TDbContext>(
       IServiceProvider serviceProvider,
       Action<IServiceProvider, ZDbContextOptionsBuilder>? optionsBuilder,
       bool enableSoftDelete)
       where TDbContext : DbContext
    {
        var masaDbContextOptionsBuilder = new ZDbContextOptionsBuilder<TDbContext>(serviceProvider, enableSoftDelete);
        optionsBuilder?.Invoke(serviceProvider, masaDbContextOptionsBuilder);

        return masaDbContextOptionsBuilder.ZOptions;
    }


    /// <summary>
    /// 获取Options并且注入
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureOptions(IServiceCollection services)
    {
        //var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        //services.Configure<ZDbContextOptions>(configuration.GetSection(ZDbContextOptions.Name));
    }

    /// <summary>
    /// 使用自动工作单元中间件
    /// </summary>
    /// <param name="app"></param>
    public static void UseUnitOfWorkMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<UnitOfWorkMiddleware>();
    }

    /// <summary>
    /// 注入工作单元中间件
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddUnitOfWorkMiddleware(this IServiceCollection services)
    {
        services.AddTransient<UnitOfWorkMiddleware>();
        return services;
    }
}
