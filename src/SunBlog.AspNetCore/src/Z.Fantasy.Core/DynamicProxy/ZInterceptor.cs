using System.Threading.Tasks;

namespace Z.Fantasy.Core.DynamicProxy;

public abstract class ZInterceptor : IZInterceptor
{
    /// <summary>
    /// 异步方法
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    public abstract Task InterceptAsync(IZMethodInvocation invocation);
}
