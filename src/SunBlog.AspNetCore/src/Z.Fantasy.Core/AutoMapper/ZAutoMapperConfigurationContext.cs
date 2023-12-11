using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.AutoMapper
{
    public class ZAutoMapperConfigurationContext : IZAutoMapperConfigurationContext
    {
        public IMapperConfigurationExpression MapperConfiguration { get; }
        public IServiceProvider ServiceProvider { get; }

        public ZAutoMapperConfigurationContext(
            IMapperConfigurationExpression mapperConfigurationExpression,
            IServiceProvider serviceProvider)
        {
            MapperConfiguration = mapperConfigurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}
