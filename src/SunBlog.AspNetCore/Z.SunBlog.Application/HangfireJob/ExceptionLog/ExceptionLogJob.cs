using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Z.Foundation.Core.AutofacExtensions;
using Z.Foundation.Core.Entities.EntityLog;
using Z.Foundation.Core.Entities.Repositories;
using Z.Foundation.Core.UnitOfWork;
using Z.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Application.HangfireJob.ExceptionLog;

/// <summary>
/// �쳣��־
/// </summary>
public class ExceptionLogJob : BackgroundScheduleJobBase, ITransientDependency
{
    /// <summary>
    /// ���캯�� ÿ5��ִ��һ��
    /// </summary>
    public ExceptionLogJob(IServiceProvider serviceProvider = null) : base(serviceProvider)
    {
        ServiceProvider = serviceProvider ?? IOCManager.GetService<IServiceProvider>();
        Id = "exceptionlogjob";
        CronExpression = "0 0 */5 * *";
    }

    /// <summary>
    /// ��������ɾ��30��ǰ���쳣��־
    /// </summary>
    /// <returns></returns>
    public override async Task DoWorkAsync()
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var unit = scope.ServiceProvider.GetService<IUnitOfWork>();
        try
        {
            await unit.BeginTransactionAsync();
            var _requestLogRepository = scope.ServiceProvider.GetRequiredService<IBasicRepository<ZExceptionLog>>();
            await _requestLogRepository.DeleteAsync(c => c.CreationTime < DateTime.Now.AddDays(-30));
            await unit.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unit.RollbackTransactionAsync();
            Log.Error( $"��ʱ����쳣��־ʧ��{ex}");
        }

        unit.Dispose();
        
    }
}
