﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.


using Z.Foundation.Core.AutofacExtensions;

namespace Z.HangFire.BackgroundJobs.Abstractions;

public class BackgroundJobManager
{
    private static IBackgroundJobManager _backgroundJobManager;

    private static IBackgroundJobManager JobManager => _backgroundJobManager ??= IOCManager.GetService<IBackgroundJobManager>();

    /// <summary>
    /// 静态直接调用job
    /// </summary>
    /// <param name="args"></param>
    /// <param name="delay"></param>
    /// <typeparam name="TArgs"></typeparam>
    /// <returns></returns>
    public static Task<string> EnqueueAsync<TArgs>(TArgs args, TimeSpan? delay = null)
        => JobManager.EnqueueAsync(args, delay);

    /// <summary>
    /// 静态直接调用工作者
    /// </summary>
    /// <param name="backgroundScheduleJob"></param>
    /// <returns></returns>
    public static async Task AddOrUpdateScheduleAsync(IBackgroundScheduleJob backgroundScheduleJob)
        => await JobManager.AddOrUpdateScheduleAsync(backgroundScheduleJob);

    internal static void ResetBackgroundJobManager()
    {
        _backgroundJobManager = null;
    }
}
