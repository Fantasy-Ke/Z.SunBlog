using System.Threading.Tasks;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public interface IBackgroundJobExecuter
{
    Task ExecuteAsync(JobExecutionContext context);
}
