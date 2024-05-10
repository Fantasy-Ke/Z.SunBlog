using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Z.Foundation.Core.AutofacExtensions;
using Z.Foundation.Core.Entities.EntityLog;
using Z.Foundation.Core.Entities.Repositories;
using Z.Foundation.Core.UnitOfWork;
using Z.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireJob.RequestLog;

/// <summary>
/// ������־����
/// </summary>
public class RequestLogJob : BackgroundScheduleJobBase, ITransientDependency
{
    /// <summary>
    /// ���캯�� ÿ��23��ִ��
    /// </summary>
    public RequestLogJob(IServiceProvider serviceProvider = null) : base(serviceProvider)
    {
        ServiceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "requestlogjob";
        CronSeqs = TimeSpan.FromHours(23).TotalMilliseconds;
    }

    /// <summary>
    /// �������� ɾ��5��ǰ��������־
    /// </summary>
    /// <returns></returns>
    public override async Task DoWorkAsync()
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
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
            Log.Error($"��ʱ���������־ʧ��{ex}");
        }

        unit.Dispose();

    }
}
