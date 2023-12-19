// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Hangfire;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public interface IBackgroundScheduleJob
{
    public string Id { get; set; }

    public double CronSeqs { get; set; }

    Task ExecuteAsync(IServiceProvider serviceProvider);

    RecurringJobOptions JobOptions { get; set; }

    string Queue { get; set; }
}
