using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.AutoMapper
{
    public static class MapperHepler
    {
        public static Mapper CreateMappings(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<ZAutoMapperOptions>>().Value;

                void ConfigureAll(IZAutoMapperConfigurationContext ctx)
                {
                    foreach (var configurator in options.Configurators)
                    {
                        configurator(ctx);
                    }
                }

                options.Configurators.Insert(0, ctx => ctx.MapperConfiguration.ConstructServicesUsing(serviceProvider.GetService));


                //Profile文件
                //void ValidateAll(IConfigurationProvider config)
                //{
                //    foreach (var profileType in options.ValidatingProfiles)
                //    {
                //        config.AssertConfigurationIsValid(((Profile)Activator.CreateInstance(profileType)).ProfileName);
                //    }
                //}

                var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
                {
                    ConfigureAll(new ZAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                });

                //Profile文件
                //ValidateAll(mapperConfiguration);

                return new Mapper(mapperConfiguration);
            }
        }
    }
}
