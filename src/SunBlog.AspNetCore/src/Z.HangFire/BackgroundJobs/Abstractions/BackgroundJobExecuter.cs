﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Z.Foundation.Core.Exceptions;
using Z.Module.DependencyInjection;

namespace Z.HangFire.BackgroundJobs.Abstractions;

public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransientDependency
{
    public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

    public BackgroundJobExecuter()
    {
        Logger = NullLogger<BackgroundJobExecuter>.Instance;
    }

    public virtual async Task ExecuteAsync(JobExecutionContext context)
    {
        var job = context.ServiceProvider.GetService(context.JobType);
        if (job == null)
        {
            throw new UserFriendlyException(
                "The job type is not registered to DI: " + context.JobType
            );
        }

        var jobExecuteMethod =
            context.JobType.GetMethod(nameof(IBackgroundJob<object>.Execute))
            ?? context.JobType.GetMethod(nameof(IAsyncBackgroundJob<object>.ExecuteAsync));
        if (jobExecuteMethod == null)
        {
            throw new UserFriendlyException(
                $"Given job type does not implement {typeof(IBackgroundJob<>).Name} or {typeof(IAsyncBackgroundJob<>).Name}. "
                    + "The job type was: "
                    + context.JobType
            );
        }

        try
        {
            if (context.CancellationToken.IsCancellationRequested)
                context.CancellationToken.ThrowIfCancellationRequested();
            if (jobExecuteMethod.Name == nameof(IAsyncBackgroundJob<object>.ExecuteAsync))
            {
                await (Task)jobExecuteMethod.Invoke(job, new[] { context.JobArgs })!;
            }
            else
            {
                jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
        }
    }
}
