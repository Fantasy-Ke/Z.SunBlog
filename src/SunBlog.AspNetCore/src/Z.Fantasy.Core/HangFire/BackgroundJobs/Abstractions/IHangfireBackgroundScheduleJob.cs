
using Hangfire;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public interface IHangfireBackgroundScheduleJob : IBackgroundScheduleJob
{
    RecurringJobOptions JobOptions { get; set; }

    string Queue { get; set; }
}
