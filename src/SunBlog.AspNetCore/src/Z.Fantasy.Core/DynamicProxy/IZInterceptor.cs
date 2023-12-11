using System.Threading.Tasks;

namespace Z.Fantasy.Core.DynamicProxy;

/// <summary>
/// 异步拦截器
/// </summary>
public interface IZInterceptor
{
    /// <summary>
    /// 异步方法
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    Task InterceptAsync(IZMethodInvocation invocation);
}
