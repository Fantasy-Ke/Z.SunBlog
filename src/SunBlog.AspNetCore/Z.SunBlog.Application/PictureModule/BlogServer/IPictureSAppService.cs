﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.ResultResponse.Pager;
using Z.SunBlog.Application.PictureModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.PictureModule.BlogServer
{
    /// <summary>
    /// 相册管理
    /// </summary>
    public interface IPictureSAppService : IApplicationService
    {
        Task<PageResult<PicturesPageOutput>> GetPage([FromBody] PicturesPageQueryInput dto);

        Task AddPictures(AddPictureInput dto);

        Task Delete(KeyDto dto);

    }
}
