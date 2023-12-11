using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.UnitOfWork;
using Z.EntityFrameworkCore.Attributes;
using Z.EntityFrameworkCore.Options;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.Exceptions;

namespace Z.EntityFrameworkCore.Middlewares;

public class UnitOfWorkMiddleware : IMiddleware
{
    private readonly UnitOfWorkOptions _unitOfWorkOptions;

    public UnitOfWorkMiddleware(IOptions<UnitOfWorkOptions> unitOfWorkOptions)
    {
        _unitOfWorkOptions = unitOfWorkOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!_unitOfWorkOptions.Enable || IsIgnoredUrl(context))
        {
            await next(context).ConfigureAwait(false);
        }
        var unitOfWorkAttribute = context.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
            .GetMetadata<DisabledUnitOfWorkAttribute>();

        if (unitOfWorkAttribute?.Disabled == true)
        {
            await next.Invoke(context).ConfigureAwait(false);
        }
        else
        {
            // 获取服务中多个DbContext
            var unitOfWorks = context.RequestServices.GetServices<IUnitOfWork>();
            foreach (var unitOfWork in unitOfWorks)
            {
                // 开启事务
                await unitOfWork.BeginTransactionAsync();
            }
            try
            {
                await next.Invoke(context);

                foreach (var unitOfWork in unitOfWorks)
                {
                    // 提交事务
                    await unitOfWork.CommitTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                foreach (var d in unitOfWorks)
                {
                    await d.RollbackTransactionAsync();
                }
                throw new UserFriendlyException(ex.ToString());
            }
        }
    }

    private bool IsIgnoredUrl(HttpContext context) => context.Request.Path.Value != null &&
                                                      _unitOfWorkOptions.IgnoredUrl.Any(
                                                          x =>
                                                              context.Request.Path.Value.StartsWith(x));
}
