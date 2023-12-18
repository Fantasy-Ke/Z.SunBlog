using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

public abstract class AsyncBackgroundJob<TArgs> : IAsyncBackgroundJob<TArgs>
{

    public ILogger<AsyncBackgroundJob<TArgs>> Logger { get; set; }

    protected AsyncBackgroundJob()
    {
        Logger = NullLogger<AsyncBackgroundJob<TArgs>>.Instance;
    }

    public abstract Task ExecuteAsync(TArgs args);
}
