using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// 判断类型是否为Nullable类型
    /// </summary>
    /// <param name="type"> 要处理的类型 </param>
    /// <returns> 是返回True，不是返回False </returns>
    public static bool IsNullableType(this Type type)
    {
        return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
    /// <summary>
    /// 通过类型转换器获取Nullable类型的基础类型
    /// </summary>
    /// <param name="type"> 要处理的类型对象 </param>
    /// <returns> </returns>
    public static Type GetUnNullableType(this Type type)
    {
        if (type.IsNullableType())
        {
            NullableConverter nullableConverter = new(type);
            return nullableConverter.UnderlyingType;
        }
        return type;
    }
}
