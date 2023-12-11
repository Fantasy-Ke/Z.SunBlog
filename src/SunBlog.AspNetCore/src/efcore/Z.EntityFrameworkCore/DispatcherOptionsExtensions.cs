// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.UnitOfWork;
using Z.EntityFrameworkCore.Extensions;
using Z.Module;

namespace Z.EntityFrameworkCore;

public static class DispatcherOptionsExtensions
{
    public static ServiceConfigerContext UseRepository<TDbContext>(
        this ServiceConfigerContext options,
        params Type[] entityTypes)
        where TDbContext : DbContext
        => options.UseRepository<TDbContext>(entityTypes.Length == 0 ? null : (IEnumerable<Type>)entityTypes);

    public static ServiceConfigerContext UseRepository<TDbContext>(
        this ServiceConfigerContext options,
        IEnumerable<Type>? entityTypes)
        where TDbContext : DbContext
    {

        if (options.Services.Any(service => service.ImplementationType == typeof(RepositoryProvider)))
            return options;

        options.Services.AddSingleton<RepositoryProvider>();

        //if (options.Services.All(service => service.ServiceType != typeof(IUnitOfWork)))
        //    throw new Exception("Please add UoW first.");

        options.Services.TryAddRepository<TDbContext>(options.GetAssemblies().Distinct(), entityTypes);
        return options;
    }

    private sealed class RepositoryProvider
    {

    }
}
