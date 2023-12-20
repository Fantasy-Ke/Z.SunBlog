using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.Entities.EntityLog;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Fantasy.Core.UnitOfWork;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireWork.TestWork;

/// <summary>
/// 请求日志清理
/// </summary>
public class RequestLogJob : BackgroundScheduleJobBase, ITransientDependency
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public RequestLogJob(IServiceProvider serviceProvider = null) : base(serviceProvider)
    {
        ServiceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "requestlogjob";
        CronSeqs = TimeSpan.FromHours(23).TotalMilliseconds;
    }

    /// <summary>
    /// 重新任务
    /// </summary>
    /// <returns></returns>
    public override async Task DoWorkAsync()
    {
        using var scope = ServiceProvider.CreateAsyncScope();
        using var unit = scope.ServiceProvider.GetService<IUnitOfWork>();
        try
        {
            await unit.BeginTransactionAsync();
            var _requestLogRepository = scope.ServiceProvider.GetRequiredService<IBasicRepository<ZRequestLog>>();
            await _requestLogRepository.DeleteAsync(c => c.CreationTime < DateTime.Now.AddDays(-5));
            await unit.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unit.RollbackTransactionAsync();
            Log.Error( $"定时清除请求日志失败{ex}");
        }

        unit.Dispose();
        
    }
}
