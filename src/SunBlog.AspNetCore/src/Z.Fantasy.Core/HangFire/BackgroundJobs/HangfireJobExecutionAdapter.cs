﻿using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            throw new Exception(
                "Background job execution is disabled. " +
                "This method should not be called! " +
                "If you want to enable the background job execution, " +
                $"set {nameof(ZBackgroundJobOptions)}.{nameof(ZBackgroundJobOptions.IsJobExecutionEnabled)} to true! " +
                "If you've intentionally disabled job execution and this seems a bug, please report it."
            );
        }

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var jobType = Options.GetJob(typeof(TArgs)).JobType;
            var context = new JobExecutionContext(scope.ServiceProvider, jobType, args!, cancellationToken: cancellationToken);
            await JobExecuter.ExecuteAsync(context);
        }
    }
}
