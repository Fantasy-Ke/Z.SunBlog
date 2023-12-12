using System;
using System.Collections.Generic;
using System.Linq;
using Z.Module.Extensions;

namespace Z.Fantasy.Core.DynamicProxy;

/// <summary>
/// autofac忽略拦截注入
/// </summary>
public static class DynamicProxyIgnoreTypes
{
    private static HashSet<Type> IgnoredTypes { get; } = new HashSet<Type>();

    public static void Add<T>()
    {
        lock (IgnoredTypes)
        {
            IgnoredTypes.AddIfNotContains(typeof(T));
        }
    }

    public static bool Contains(Type type, bool includeDerivedTypes = true)
    {
        lock (IgnoredTypes)
        {
            return includeDerivedTypes
                ? IgnoredTypes.Any(t => t.IsAssignableFrom(type))
                : IgnoredTypes.Contains(type);
        }
    }
}
