using System;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

/// <summary>
/// Defines interface of a job manager.
/// </summary>
public interface IBackgroundJobManager
{
    /// <summary>
    /// hangfire队列任务
    /// </summary>
    /// <typeparam name="TArgs">参数Jobs.</typeparam>
    /// <param name="args">参数Jobs</param>
    ///<param name="delay">任务延期时间，默认null，队列直接执行，给值就延迟执行</param>
    /// <param name="priority">优先级，暂时没用.</param>

    Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        TimeSpan? delay = null,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal
    );
}
