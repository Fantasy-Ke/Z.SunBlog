using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs;

public class HangfireJobExecutionAdapter<TArgs>
{
    protected ZBackgroundJobOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }

    public HangfireJobExecutionAdapter(
        IOptions<ZBackgroundJobOptions> options,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;
    }

    [Queue("{0}")]
    public async Task ExecuteAsync(string queue, TArgs args, CancellationToken cancellationToken = default)
    {
        if (!Options.IsJobExecutionEnabled)
        {
            throw new UserFriendlyException(
                $"Job没有启用,修改 {nameof(ZBackgroundJobOptions.IsJobExecutionEnabled)} to true! "
            );
        }

        using var scope = ServiceScopeFactory.CreateAsyncScope();
        var jobType = Options.GetJob(typeof(TArgs)).JobType;
        var context = new JobExecutionContext(scope.ServiceProvider, jobType, args!, cancellationToken: cancellationToken);
        await JobExecuter.ExecuteAsync(context);
    }
}
