using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class EnumNameAttribute : Attribute
{
    public static readonly EnumNameAttribute Default = new EnumNameAttribute();

    public virtual string NameValue => EnumNameValue;

    protected string EnumNameValue { get; set; }

    public EnumNameAttribute()
        : this(string.Empty)
    {
    }

    public EnumNameAttribute(string enumName)
    {
        EnumNameValue = enumName;
    }

    public override bool Equals(object obj)
    {
        if (obj is EnumNameAttribute enumNameAttribute)
        {
            return enumNameAttribute.EnumNameValue == NameValue;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return NameValue?.GetHashCode() ?? 0;
    }

    public override bool IsDefaultAttribute()
    {
        return Equals(Default);
    }
}
