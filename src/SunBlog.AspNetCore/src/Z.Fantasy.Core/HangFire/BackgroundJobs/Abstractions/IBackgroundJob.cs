using System.Threading;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

/// <summary>
/// Defines interface of a background job.
/// </summary>
public interface IBackgroundJob<in TArgs>
{
    /// <summary>
    /// Executes the job with the <paramref name="args"/>.
    /// </summary>
    /// <param name="args">Job arguments.</param>
    void Execute(TArgs args);
}
