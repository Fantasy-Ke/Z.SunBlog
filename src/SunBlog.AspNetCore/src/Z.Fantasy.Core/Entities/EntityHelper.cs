using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Z.Fantasy.Core.Helper;

namespace Z.Fantasy.Core.Entities;

/// <summary>
/// Some helper methods for entities.
/// </summary>
public static class EntityHelper
{
    public static bool IsEntity(Type type)
    {
        return typeof(IEntity).IsAssignableFrom(type);
    }


    public static bool IsValueObject(object obj)
    {
        return obj != null && IsValueObject(obj.GetType());
    }

    public static void CheckEntity(Type type)
    {
        if (!IsEntity(type))
        {
            throw new Exception($"Given {nameof(type)} is not an entity: {type.AssemblyQualifiedName}. It must implement {typeof(IEntity).AssemblyQualifiedName}.");
        }
    }

    public static bool IsEntityWithId(Type type)
    {
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasDefaultId<TKey>(IEntity<TKey> entity)
    {
        if (EqualityComparer<TKey>.Default.Equals(entity.Id, default))
        {
            return true;
        }

        //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
        if (typeof(TKey) == typeof(int))
        {
            return Convert.ToInt32(entity.Id) <= 0;
        }

        if (typeof(TKey) == typeof(long))
        {
            return Convert.ToInt64(entity.Id) <= 0;
        }

        return false;
    }

    public static void TrySetId<TKey>(
        IEntity<TKey> entity,
        Func<TKey> idFactory,
        bool checkForDisableIdGenerationAttribute = false)
    {
        ObjectPropertyHelper.TrySetProperty(
            entity,
            x => x.Id,
            idFactory, new Type[] { });
    }
}
