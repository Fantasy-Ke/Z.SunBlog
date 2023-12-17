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
    /// ����Hangfire
    /// </summary>
    /// <param name="app"></param>
    public static void UseHangfire(this IApplicationBuilder app)
    {
        // TODO: �ж��Ƿ����� HangfireDashboard
       //���÷���������Դ���ֵ
       GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
        //����Hangfire�Ǳ��̺ͷ�����(֧��ʹ��Hangfire������Ĭ�ϵĺ�̨��ҵ������)
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DefaultRecordsPerPage = 10,
            DarkModeEnabled = true,
            DashboardTitle = "Fantasy_ke Hangfire",
        });
        
    }

}