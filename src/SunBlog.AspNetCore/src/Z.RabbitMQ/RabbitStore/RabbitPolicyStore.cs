using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Z.RabbitMQ.RabbitStore;

/// <summary>
/// 重试策略存储器
/// </summary>
public interface IRabbitPolicyStore : IDisposable
{
    /// <summary>
    /// 获取重试策略
    /// </summary>
    /// <param name="configName">配置名称</param>
    /// <returns></returns>
    AsyncRetryPolicy GetRetryPolicy(string configName = null);

    /// <summary>
    /// 配置重试策略
    /// </summary>
    /// <param name="configName">配置名称</param>
    /// <param name="retryPolicy">重试策略</param>
    void SetRetryPolicy(string configName, AsyncRetryPolicy retryPolicy);
}

public class RabbitPolicyStore : IRabbitPolicyStore
{
    protected readonly ILogger<IRabbitPolicyStore> _logger;
    protected readonly ConcurrentDictionary<string, AsyncRetryPolicy> _dataDict;
    protected readonly AsyncRetryPolicy _defaultRtryPolicy;

    public RabbitPolicyStore(ILogger<IRabbitPolicyStore> logger = null)
    {
        _logger = logger;

        _dataDict = new ConcurrentDictionary<string, AsyncRetryPolicy>();

        _defaultRtryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                (retryAttempt) => //一直重试策略
                {
                    // 指数退避策略
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                },
                (ex, time) =>
                {
                    _logger.LogError(
                        $"Failed to connect to RabbitMQ! Retrying after {time.TotalSeconds} seconds。 Reason: {ex.Message} ...",
                        ex
                    );
                }
            );
    }

    public void SetRetryPolicy(string configName, AsyncRetryPolicy retryPolicy)
    {
        _dataDict.AddOrUpdate(
            configName,
            retryPolicy,
            (key, OldVal) =>
            {
                return retryPolicy;
            }
        );
    }

    public virtual AsyncRetryPolicy GetRetryPolicy(string configName = null)
    {
        if (string.IsNullOrWhiteSpace(configName))
        {
            return _defaultRtryPolicy;
        }

        if (!_dataDict.TryGetValue(configName, out var val))
        {
            val = _defaultRtryPolicy;
        }

        return val;
    }

    public void Dispose()
    {
        _dataDict.Clear();
    }
}
