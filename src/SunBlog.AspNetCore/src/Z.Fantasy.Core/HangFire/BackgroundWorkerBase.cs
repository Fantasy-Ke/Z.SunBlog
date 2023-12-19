using System;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Z.Fantasy.Core.Exceptions;

namespace Z.Fantasy.Core.HangFire;

public abstract class BackgroundWorkerBase
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected string RecurringJobId { get; set; }

    protected int Period { get; set; }

    readonly ILogger _logger;


    protected RecurringJobOptions RecurringJobOptions { get; set; } = new RecurringJobOptions();


    protected BackgroundWorkerBase(IServiceScopeFactory serviceScopeFactory, string RecurringJobId, int Period)
    {
        this.RecurringJobId = RecurringJobId;
        this.Period = Period;
        ServiceScopeFactory = serviceScopeFactory;
        _logger = NullLogger<BackgroundWorkerBase>.Instance;
    }

    public void AddOrUpdateWork()
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            try
            {
                RecurringJob.AddOrUpdate(
                        RecurringJobId,
                        () => DoWork(),
                        GetCron(Period), RecurringJobOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }

    protected virtual string GetCron(int period)
    {
        var time = TimeSpan.FromMilliseconds(period);
        string cron;

        if (time.TotalSeconds <= 59)
        {
            cron = $"*/{time.TotalSeconds} * * * * *";
        }
        else if (time.TotalMinutes <= 59)
        {
            cron = $"*/{time.TotalMinutes} * * * *";
        }
        else if (time.TotalHours <= 23)
        {
            cron = $"0 */{time.TotalHours} * * *";
        }
        else
        {
            throw new UserFriendlyException(
                $"Cannot convert period: {period} to cron expression, use HangfireBackgroundWorkerBase to define worker");
        }

        return cron;
    }

    /// <summary>
    /// Periodic works should be done by implementing this method.
    /// </summary>
    protected abstract Task DoWork();
}
