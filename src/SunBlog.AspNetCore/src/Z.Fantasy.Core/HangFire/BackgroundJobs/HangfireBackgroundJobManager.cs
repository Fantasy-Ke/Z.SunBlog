using System.Reflection;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs;

[RegisterLife(ReplaceServices = true)]
public class HangFireBackgroundJobManager : IBackgroundJobManager, ISingletonDependency
{
    protected ZBackgroundJobOptions Options { get; }
    private readonly IServiceProvider _serviceProvider;

    public HangFireBackgroundJobManager(IOptions<ZBackgroundJobOptions> options, IServiceProvider serviceProvider)
    {
        Options = options.Value;
        _serviceProvider = serviceProvider;
    }

    public virtual Task<string> EnqueueAsync<TArgs>(TArgs args,TimeSpan? delay = null)
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


    public Task AddOrUpdateScheduleAsync(IBackgroundScheduleJob backgroundScheduleJob)
    {
        if (backgroundScheduleJob is BackgroundScheduleJobBase hangfireBackgroundScheduleJob)
        {
            if (backgroundScheduleJob.Id.IsNullWhiteSpace())
                throw new UserFriendlyException(errorCode: "recurringJobId不能为空");

            RecurringJob.AddOrUpdate(
                hangfireBackgroundScheduleJob.Id,
                hangfireBackgroundScheduleJob.Queue,
                () => hangfireBackgroundScheduleJob.DoWorkAsync(),
                hangfireBackgroundScheduleJob.CronExpression ?? GetCron(hangfireBackgroundScheduleJob.CronSeqs),
                hangfireBackgroundScheduleJob.JobOptions
                );
            return Task.CompletedTask;
        }

        throw new UserFriendlyException(errorCode: "500");
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


    protected virtual string GetCron(double period)
    {
        var time = TimeSpan.FromMilliseconds(period);
        string cron;

        if (time.TotalSeconds <= 59)
        {
            cron = $"*/p{time.TotalSeconds.CastTo(0)} * * * * *";
        }
        else if (time.TotalMinutes <= 59)
        {
            cron = $"*/{time.TotalMinutes.CastTo(0)} * * * *";
        }
        else if (time.TotalHours <= 23)
        {
            cron = $"0 */{time.TotalHours.CastTo(0)} * * *";
        }
        else
        {
            throw new UserFriendlyException(
                $"Cannot convert period: {period} to cron expression, use HangfireBackgroundWorkerBase to define worker");
        }

        return cron;
    }
}
