using System.Linq;
using System.Threading;
using Hangfire;
using Microsoft.Extensions.Options;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs;

public class ZDashboardOptionsProvider : ITransientDependency
{
    protected ZBackgroundJobOptions ZBackgroundJobOptions { get; }

    public ZDashboardOptionsProvider(IOptions<ZBackgroundJobOptions> zBackgroundJobOptions)
    {
        ZBackgroundJobOptions = zBackgroundJobOptions.Value;
    }

    public virtual DashboardOptions Get()
    {
        return new DashboardOptions
        {
            DisplayNameFunc = (_, job) =>
            {
                var jobName = job.ToString();
                if (job.Args.Count == 3 && job.Args.Last() is CancellationToken)
                {
                    jobName = ZBackgroundJobOptions.GetJob(job.Args[1].GetType()).JobName;
                }

                return jobName;
            }
        };
    }
}
