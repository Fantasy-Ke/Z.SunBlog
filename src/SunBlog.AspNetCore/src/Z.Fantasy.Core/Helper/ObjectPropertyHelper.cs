using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Extensions;

namespace Z.Fantasy.Core.Helper;

public static class ObjectPropertyHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> CachedObjectProperties =
   new ConcurrentDictionary<string, PropertyInfo>();

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        TrySetProperty(obj, propertySelector, x => valueFactory());
    }

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TObject, TValue> valueFactory)
    {
        var cacheKey = $"{obj?.GetType().FullName}-" +
                       $"{propertySelector}";

        var property = CachedObjectProperties.GetOrAdd(cacheKey, (c) =>
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
            {
                return null;
            }
            //转换成属性
            var memberExpression = propertySelector.Body.As<MemberExpression>();


            //获取属性上有传入的委托属性&&  set 是公共的
            var propertyInfo = obj?.GetType().GetProperties().FirstOrDefault(x =>
                x.Name == memberExpression.Member.Name &&
                x.GetSetMethod(true) != null);

            if (propertyInfo == null)
            {
                return null;
            }

            return propertyInfo;
        });

        //修改值

        property?.SetValue(obj, valueFactory(obj));
    }



}
