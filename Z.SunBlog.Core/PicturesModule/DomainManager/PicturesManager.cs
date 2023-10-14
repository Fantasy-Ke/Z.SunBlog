﻿using Z.Ddd.Common.DomainServiceRegister;

namespace Z.SunBlog.Core.PicturesModule.DomainManager
{
    public class PicturesManager : BusinessDomainService<Pictures>, IPicturesManager
    {
        public PicturesManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(Pictures entity)
        {
            await Task.CompletedTask;
        }

    }
}
