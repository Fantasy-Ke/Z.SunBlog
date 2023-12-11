using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Z.Fantasy.Application.Middleware;
using Z.Fantasy.Core;
using Z.Fantasy.Core.AutoMapper;
using Z.Fantasy.Core.ResultResponse;
using Z.Module;
using Z.Module.Modules;

namespace Z.Fantasy.Application
{
    [DependOn(typeof(ZFantasyCoreModule))]
    public class ZFantasyApplicationModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            //注册AutoMapper
            context.Services.AddSingleton(MapperHepler.CreateMappings);
            context.Services.AddSingleton<IMapper>(provider => provider.GetRequiredService<Mapper>());
            context.Services.AddControllers(c =>
            {
                c.Filters.Add<ModelValidateActionFilterAttribute>();
            });
        }

    }
}