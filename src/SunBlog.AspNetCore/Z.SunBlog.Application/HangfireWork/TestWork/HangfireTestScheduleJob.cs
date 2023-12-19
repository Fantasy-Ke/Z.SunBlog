using Hangfire;
using Serilog;
using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireWork.TestWork;

/// <summary>
/// ��̨�������
/// </summary>
public class HangfireTestScheduleJob : BackgroundScheduleJobBase, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// ���캯��
    /// </summary>
    public HangfireTestScheduleJob(IServiceProvider serviceProvider = null)
    {
        _serviceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "hangfiretest";
        CronSeqs = TimeSpan.FromMinutes(1).TotalMilliseconds;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    public override Task DoWorkAsync()
    {
        Log.Error("����-------------------------------------------------------------����\n" +
            "����-------------------------------------------------------------����\n" +
            "����-------------------------------------------------------------����");

        return Task.CompletedTask;
    }
}
