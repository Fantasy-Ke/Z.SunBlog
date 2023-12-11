// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Z.Fantasy.Core.Entities.Repositories;

public interface IZDbContextBuilder
{
    public bool EnableSoftDelete { get; set; }

    public IServiceCollection Services { get; }
}
