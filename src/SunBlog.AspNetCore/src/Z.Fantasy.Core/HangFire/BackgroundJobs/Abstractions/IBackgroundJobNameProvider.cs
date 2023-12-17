namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public interface IBackgroundJobNameProvider
{
    string Name { get; }
}
