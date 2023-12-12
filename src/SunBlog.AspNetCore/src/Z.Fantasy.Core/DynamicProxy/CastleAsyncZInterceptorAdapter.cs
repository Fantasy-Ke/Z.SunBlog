using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Z.Fantasy.Core.DynamicProxy;

public class CastleAsyncZInterceptorAdapter<TInterceptor> : AsyncInterceptorBase
    where TInterceptor : IZInterceptor
{
    private readonly TInterceptor _abpInterceptor;

    public CastleAsyncZInterceptorAdapter(TInterceptor abpInterceptor)
    {
        _abpInterceptor = abpInterceptor;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        await _abpInterceptor.InterceptAsync(
            new CastleAbpMethodInvocationAdapter(invocation, proceedInfo, proceed)
        );
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var adapter = new CastleZMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

        await _abpInterceptor.InterceptAsync(
            adapter
        );

        return (TResult)adapter.ReturnValue;
    }
}
