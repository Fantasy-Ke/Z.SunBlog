using System;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.HangFire.BackgroundJobs.Abstractions;

/// <summary>
/// Defines interface of a job manager.
/// </summary>
public interface IBackgroundJobManager
{
    /// <summary>
    /// hangfire��������
    /// </summary>
    /// <typeparam name="TArgs">����Jobs.</typeparam>
    /// <param name="args">����Jobs</param>
    ///<param name="delay">��������ʱ�䣬Ĭ��null������ֱ��ִ�У���ֵ���ӳ�ִ��</param>

    Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        TimeSpan? delay = null
    );
}
