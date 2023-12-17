using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public class ZBackgroundJobOptions
{
    private readonly Dictionary<Type, BackgroundJobConfiguration> _jobConfigurationsByArgsType;
    private readonly Dictionary<string, BackgroundJobConfiguration> _jobConfigurationsByName;

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsJobExecutionEnabled { get; set; } = true;

    public ZBackgroundJobOptions()
    {
        _jobConfigurationsByArgsType = new Dictionary<Type, BackgroundJobConfiguration>();
        _jobConfigurationsByName = new Dictionary<string, BackgroundJobConfiguration>();
    }

    public BackgroundJobConfiguration GetJob<TArgs>()
    {
        return GetJob(typeof(TArgs));
    }

    public BackgroundJobConfiguration GetJob(Type argsType)
    {
        BackgroundJobConfiguration jobConfiguration;
        _jobConfigurationsByArgsType.TryGetValue(argsType, out jobConfiguration);

        if (jobConfiguration == null)
        {
            throw new ArgumentException("Undefined background job for the job args type: " + argsType.AssemblyQualifiedName);
        }

        return jobConfiguration;
    }

    public BackgroundJobConfiguration GetJob(string name)
    {
        BackgroundJobConfiguration jobConfiguration;
        _jobConfigurationsByName.TryGetValue(name, out jobConfiguration);

        if (jobConfiguration == null)
        {
            throw new ArgumentException("Undefined background job for the job name: " + name);
        }

        return jobConfiguration;
    }

    public IReadOnlyList<BackgroundJobConfiguration> GetJobs()
    {
        return _jobConfigurationsByArgsType.Values.ToImmutableList();
    }

    public void AddJob<TJob>()
    {
        AddJob(typeof(TJob));
    }

    public void AddJob(Type jobType)
    {
        AddJob(new BackgroundJobConfiguration(jobType));
    }

    public void AddJob(BackgroundJobConfiguration jobConfiguration)
    {
        _jobConfigurationsByArgsType[jobConfiguration.ArgsType] = jobConfiguration;
        _jobConfigurationsByName[jobConfiguration.JobName] = jobConfiguration;
    }
}
