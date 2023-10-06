﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.AutoMapper
{
    public interface IZAutoMapperConfigurationContext
    {
        IMapperConfigurationExpression MapperConfiguration { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
