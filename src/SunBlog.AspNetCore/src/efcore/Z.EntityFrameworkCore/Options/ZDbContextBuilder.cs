// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Z.Fantasy.Core.Entities.Repositories;

namespace Z.EntityFrameworkCore.Options;

public class ZDbContextBuilder : IZDbContextBuilder
{
    public IServiceCollection Services { get; }

    public Type DbContextType { get; }

    public Action<IServiceProvider, DbContextOptionsBuilder>? Builder { get; set; }

    public List<Action<DbContextOptionsBuilder>> DbContextOptionsBuilders { get; } = new();

    public bool EnableSoftDelete { get; set; }

    public ZDbContextBuilder(IServiceCollection services, Type dbContextType)
    {
        Services = services;
        DbContextType = dbContextType;
    }
}
