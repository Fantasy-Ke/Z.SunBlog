using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Z.Fantasy.Core.Entities.EntityLog;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.UnitOfWork;
using Z.Foundation.Core.AutofacExtensions;
using Z.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireJob.ExceptionLog;

/// <summary>
/// �쳣��־
/// </summary>
public class ExceptionLogJob : BackgroundScheduleJobBase, ITransientDependency
{
    /// <summary>
    /// ���캯��
    /// </summary>
    public ExceptionLogJob(IServiceProvider serviceProvider = null) : base(serviceProvider)
    {
        ServiceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "exceptionlogjob";
        CronExpression = "0 0 */5 * *";
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    public override async Task DoWorkAsync()
    {
        using var scope = ServiceProvider.CreateAsyncScope();
        using var unit = scope.ServiceProvider.GetService<IUnitOfWork>();
        try
        {
            await unit.BeginTransactionAsync();
            var _requestLogRepository = scope.ServiceProvider.GetRequiredService<IBasicRepository<ZExceptionLog>>();
            await _requestLogRepository.DeleteAsync(c => c.CreationTime < DateTime.Now.AddDays(-5));
            await unit.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unit.RollbackTransactionAsync();
            Log.Error( $"��ʱ���������־ʧ��{ex}");
        }

        unit.Dispose();
        
    }
}
