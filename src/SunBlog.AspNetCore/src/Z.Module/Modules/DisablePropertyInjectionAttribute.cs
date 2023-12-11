using System;

namespace Z.Module.Modules;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DisablePropertyInjectionAttribute : Attribute
{

}
