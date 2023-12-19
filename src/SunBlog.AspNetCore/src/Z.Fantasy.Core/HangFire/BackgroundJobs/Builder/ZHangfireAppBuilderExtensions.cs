using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.Helper;
using Z.Module.Reflection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Builder;

public static class ZHangfireAppBuilderExtensions
{
    /// <summary>
    /// ����Hangfire
    /// </summary>
    /// <param name="app"></param>
    public static void UseZHangfire(this IApplicationBuilder app,
        string pathMatch = "/hangfire",
        Action<DashboardOptions> configure = null,
        JobStorage? storage = null)
    {
        // TODO: �ж��Ƿ����� HangfireDashboard
        //���÷���������Դ���ֵ
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
        {
            Attempts = 5,
            OnAttemptsExceeded = AttemptsExceededAction.Fail
        });
        var enable = AppSettings.AppOption<bool>("App:HangFire:HangfireDashboardEnabled");
        if (!enable) return;
        var options = app.ApplicationServices.GetRequiredService<ZDashboardOptionsProvider>().Get();
        options.DefaultRecordsPerPage = 10;
        options.DarkModeEnabled = true;
        options.DashboardTitle = "Fantasy_ke Hangfire";
        configure?.Invoke(options);
        //����Hangfire�Ǳ��̺ͷ�����(֧��ʹ��Hangfire������Ĭ�ϵĺ�̨��ҵ������)
        app.UseHangfireDashboard(pathMatch, options, storage);
    }

}