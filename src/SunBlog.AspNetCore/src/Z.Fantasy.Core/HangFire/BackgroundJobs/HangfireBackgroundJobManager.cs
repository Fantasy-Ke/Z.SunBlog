using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Options;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs;

[RegisterLife(ReplaceServices = true)]
public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected ZBackgroundJobOptions Options { get; }

    public HangfireBackgroundJobManager(IOptions<ZBackgroundJobOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        return Task.FromResult(delay.HasValue
            ? BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)), args, default),
                delay.Value
            )
            : BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)), args, default)
            ));
    }

    protected virtual string GetQueueName(Type argsType)
    {
        var queueName = EnqueuedState.DefaultQueue;
        var queueAttribute = Options.GetJob(argsType).JobType.GetCustomAttribute<QueueAttribute>();
        if (queueAttribute != null)
        {
            queueName = queueAttribute.Queue;
        }

        return queueName;
    }
}
