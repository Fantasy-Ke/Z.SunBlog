using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.Reflection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Builder;

public static class ZHangfireAppBuilderExtensions
{
    public static IApplicationBuilder UseZHangfireDashboard(
        this IApplicationBuilder app,
        string pathMatch = "/hangfire",
        Action<DashboardOptions> configure = null,
        JobStorage? storage = null)
    {
        var options = app.ApplicationServices.GetRequiredService<ZDashboardOptionsProvider>().Get();
        configure?.Invoke(options);
        return app.UseHangfireDashboard(pathMatch, options, storage);
    }

    /// <summary>
    /// 启用Hangfire
    /// </summary>
    /// <param name="app"></param>
    public static void UseHangfire(this IApplicationBuilder app)
    {
        // TODO: 判断是否启用 HangfireDashboard
       //配置服务最大重试次数值
       GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
        //启用Hangfire仪表盘和服务器(支持使用Hangfire而不是默认的后台作业管理器)
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DefaultRecordsPerPage = 10,
            DarkModeEnabled = true,
            DashboardTitle = "Fantasy_ke Hangfire",
        });
        
    }

}