using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Z.Fantasy.Core.AutofacExtensions
{
    public class AutofacPropertityModuleReg : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var controllerBaseType = typeof(ControllerBase);
            //builder.RegisterAssemblyTypes(typeof(setup).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired();

        }
    }
}
