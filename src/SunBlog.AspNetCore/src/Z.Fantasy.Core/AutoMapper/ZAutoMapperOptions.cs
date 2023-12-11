using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Z.Module.Collections;

namespace Z.Fantasy.Core.AutoMapper;

public class ZAutoMapperOptions
{
    public List<Action<IZAutoMapperConfigurationContext>> Configurators { get; }

    public ITypeList<Profile> ValidatingProfiles { get; set; }

    public ZAutoMapperOptions()
    {
        Configurators = new List<Action<IZAutoMapperConfigurationContext>>();
        ValidatingProfiles = new TypeList<Profile>();
    }

    /// <summary>
    /// 模块中继承Profile文件反射获取mapper配置
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <param name="validate"></param>
    public void AddMaps<TModule>(bool validate = false)
    {
        var assembly = typeof(TModule).Assembly;

        Configurators.Add(context =>
        {
            context.MapperConfiguration.AddMaps(assembly);
        });

        if (validate)
        {
            var profileTypes = assembly
                .DefinedTypes
                .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericType);

            foreach (var profileType in profileTypes)
            {
                ValidatingProfiles.Add(profileType);
            }
        }
    }

    //public void AddProfile<TProfile>(bool validate = false)
    //    where TProfile : Profile, new()
    //{
    //    Configurators.Add(context =>
    //    {
    //        context.MapperConfiguration.AddProfile<TProfile>();
    //    });

    //    if (validate)
    //    {
    //        ValidateProfile(typeof(TProfile));
    //    }
    //}

    //public void ValidateProfile<TProfile>(bool validate = true)
    //    where TProfile : Profile
    //{
    //    ValidateProfile(typeof(TProfile), validate);
    //}

    //public void ValidateProfile(Type profileType, bool validate = true)
    //{
    //    if (validate)
    //    {
    //        ValidatingProfiles.AddIfNotContains(profileType);
    //    }
    //    else
    //    {
    //        ValidatingProfiles.Remove(profileType);
    //    }
    //}
}