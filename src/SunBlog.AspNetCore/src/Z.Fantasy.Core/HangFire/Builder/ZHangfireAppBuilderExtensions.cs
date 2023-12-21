using System;
using System.IO;
using Autofac.Core;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.DependencyInjection.Extensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.Helper;
using Z.Module.Reflection;

namespace Z.Fantasy.Core.HangFire.Builder;

public static class ZHangfireAppBuilderExtensions
{
    /// <summary>
    /// ����Hangfire
    /// </summary>
    /// <param name="app"></param>
    public static void UseZHangfire(this IApplicationBuilder app,
        string pathMatch = "/hangfire",
        Action<DashboardOptions> configure = null,
        JobStorage storage = null)
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

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static Task RegisterScheduleJobs(this IApplicationBuilder app, Action<List<Type>> configure = null)
    {
        object[] constructorArgs = {  app.ApplicationServices };
        var options = new List<Type>();
        configure?.Invoke(options);
        options?.ForEach(async res =>
        {
            if (res.IsSubclassOf(typeof(BackgroundScheduleJobBase)))
            {
                var myInstance = Activator.CreateInstance(res, constructorArgs);
                await BackgroundJobManager.AddOrUpdateScheduleAsync(myInstance as IBackgroundScheduleJob);
            }
           
        });
        return Task.CompletedTask;
    }
}