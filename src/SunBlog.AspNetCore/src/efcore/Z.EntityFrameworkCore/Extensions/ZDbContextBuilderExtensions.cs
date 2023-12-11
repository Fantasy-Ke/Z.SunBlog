// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

using Z.Fantasy.Core.Entities.Repositories;
using Z.EntityFrameworkCore.Options;

namespace Z.EntityFrameworkCore.Extensions;

public static class ZDbContextBuilderExtensions
{
    public static IZDbContextBuilder UseFilter(
        this IZDbContextBuilder masaDbContextBuilder,
        Action<FilterOptions>? options = null)
        => masaDbContextBuilder.UseFilterCore(options);

    private static IZDbContextBuilder UseFilterCore(
        this IZDbContextBuilder masaDbContextBuilder,
        Action<FilterOptions>? options = null)
    {
        var filterOptions = new FilterOptions();
        options?.Invoke(filterOptions);

        masaDbContextBuilder.EnableSoftDelete = filterOptions.EnableSoftDelete;

        return masaDbContextBuilder;
    }
}
