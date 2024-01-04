using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Attributes;

namespace Z.Fantasy.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Gets an attribute on an enum field value.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// The attribute of the specified type or null.
    /// </returns>
    public static T GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString()).First();
        var attributes = memInfo.GetCustomAttributes<T>(false);
        return attributes.FirstOrDefault();
    }

    /// <summary>
    /// Gets the enum display name.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Use <see cref="DisplayAttribute"/> if exists.
    /// Otherwise, use the standard string representation.
    /// </returns>
    public static string GetDisplayName(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<DisplayAttribute>();
        return attribute == null ? enumValue.ToString() : attribute.Name;
    }

    public static string ToNameValue(this Enum value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }

        MemberInfo memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (memberInfo != null)
        {
            return memberInfo.ToNameValue();
        }

        return value.ToString();
    }

    public static string ToNameValue(this MemberInfo member)
    {
        EnumNameAttribute attribute = member.GetAttribute<EnumNameAttribute>();
        if (attribute == null)
        {
            return member.Name;
        }

        return attribute.NameValue;
    }
}
