// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Hangfire;
using Serilog;
using Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

namespace Masa.Contrib.Extensions.BackgroundJobs.Memory;

/// <summary>
/// ��̨�������
/// </summary>
public class HangfireTestScheduleJob : IBackgroundScheduleJob
{
    public RecurringJobOptions JobOptions { get; set; }
    public string Queue { get; set; }
    public string Id { get ; set; }
    public double CronSeqs { get; set; }
    
    /// <summary>
    /// ���캯��
    /// </summary>
    public HangfireTestScheduleJob()
    {
        Id = "hangfiretest";
        CronSeqs = TimeSpan.FromMinutes(1).TotalMilliseconds;
        Queue = "cstest";
        JobOptions = new RecurringJobOptions();
    }
    
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task ExecuteAsync(IServiceProvider serviceProvider)
    {
        Log.Error("����-------------------------------------------------------------����\n" +
            "����-------------------------------------------------------------����\n" +
            "����-------------------------------------------------------------����");

        return Task.CompletedTask;
    }
}
