// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Hangfire;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public abstract class BackgroundScheduleJobBase : IBackgroundScheduleJob
{
    public virtual string Id { get; set; }

    /// <summary>
    /// 时间小于24小时的时候使用timespan传入
    /// </summary>
    public virtual double CronSeqs { get; set; }

    public virtual string CronExpression { get; set; }


    public virtual RecurringJobOptions JobOptions { get; set; } = new RecurringJobOptions();

    public virtual string Queue { get; set; } = "default";

    public abstract Task DoWorkAsync();

    public  IServiceProvider ServiceProvider { get; set; }

    public BackgroundScheduleJobBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}
