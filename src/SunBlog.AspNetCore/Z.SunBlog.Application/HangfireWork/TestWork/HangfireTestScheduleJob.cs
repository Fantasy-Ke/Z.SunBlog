using Hangfire;
using Serilog;
using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireWork.TestWork;

/// <summary>
/// 后台任务测试
/// </summary>
public class HangfireTestScheduleJob : BackgroundScheduleJobBase, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// 构造函数
    /// </summary>
    public HangfireTestScheduleJob(IServiceProvider serviceProvider = null)
    {
        _serviceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "hangfiretest";
        CronSeqs = TimeSpan.FromMinutes(1).TotalMilliseconds;
    }

    /// <summary>
    /// 重新任务
    /// </summary>
    /// <returns></returns>
    public override Task DoWorkAsync()
    {
        Log.Error("测试-------------------------------------------------------------测试\n" +
            "测试-------------------------------------------------------------测试\n" +
            "测试-------------------------------------------------------------测试");

        return Task.CompletedTask;
    }
}
