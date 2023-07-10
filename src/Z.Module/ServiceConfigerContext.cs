﻿using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Z.Module
{
    public class ServiceConfigerContext
    {
        public IServiceCollection Services { get; private set; }

        public IServiceProvider Provider
        {
            get
            {
                if (Services is null)
                {
                    throw new ArgumentNullException(nameof(Services) + "ServiceConfigerContext中Service为空");
                }
                return Services.BuildServiceProvider();
            }
        }

        public ServiceConfigerContext([NotNull] IServiceCollection services)
        {
            Services = services;
        }
    }
}