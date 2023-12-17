using Newtonsoft.Json.Linq;
using System;
using System.Linq;
namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public class BackgroundJobNameAttribute : Attribute, IBackgroundJobNameProvider
{
    public string Name { get; }

    public BackgroundJobNameAttribute(string name)
    {
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException($"{nameof(name)} can not be null, empty or white space!");
    }

    public static string GetName<TJobArgs>()
    {
        return GetName(typeof(TJobArgs));
    }

    public static string GetName(Type jobArgsType)
    {
        if (jobArgsType == null) throw new ArgumentNullException(nameof(jobArgsType));

        return (jobArgsType
                    .GetCustomAttributes(true)
                    .OfType<IBackgroundJobNameProvider>()
                    .FirstOrDefault()
                    ?.Name
                ?? jobArgsType.FullName)!;
    }
}
