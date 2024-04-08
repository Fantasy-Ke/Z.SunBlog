﻿using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Z.DynamicProxy;

public class CastleAsyncZInterceptorAdapter<TInterceptor> : AsyncInterceptorBase
    where TInterceptor : IZInterceptor
{
    private readonly TInterceptor _zInterceptor;

    public CastleAsyncZInterceptorAdapter(TInterceptor abpInterceptor)
    {
        _zInterceptor = abpInterceptor;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        await _zInterceptor.InterceptAsync(
            new CastleZMethodInvocationAdapter(invocation, proceedInfo, proceed)
        );
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var adapter = new CastleZMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

        await _zInterceptor.InterceptAsync(
            adapter
        );

        return (TResult)adapter.ReturnValue;
    }
}
