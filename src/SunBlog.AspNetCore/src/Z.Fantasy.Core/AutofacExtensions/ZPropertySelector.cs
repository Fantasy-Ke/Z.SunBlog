using System.Collections.Generic;
using System.Reflection;
using Autofac.Core;
using Z.Module.Extensions;
using Z.Module.Modules;

namespace Z.Fantasy.Core.AutofacExtensions;

public class ZPropertySelector : DefaultPropertySelector
{
    public ZPropertySelector(bool preserveSetValues)
        : base(preserveSetValues) { }

    public override bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        return propertyInfo
                .GetCustomAttributes(typeof(DisablePropertyInjectionAttribute), true)
                .IsNullOrEmpty() && base.InjectProperty(propertyInfo, instance);
    }
}
