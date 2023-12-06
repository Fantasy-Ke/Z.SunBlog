using AutoMapper;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Ddd.Common.Minio;
using Z.EventBus.Handlers;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Core.Handlers.FileHandlers
{
    public class FileEventHandler : IEventHandler<FileEventDto>, ITransientDependency
    {
        private readonly IMinioService _minioService;
        private readonly MinioConfig _minioOptions;

        public FileEventHandler(IMinioService minioService, IOptions<MinioConfig> minioOptions)
        {
            _minioService = minioService;
            _minioOptions = minioOptions.Value;
        }

        public async Task HandelrAsync(FileEventDto eto)
        {
            Log.Information("FileEventHandler: {0}", eto.File);
            var obj = new UploadObjectInput(_minioOptions.DefaultBucket
                , eto.File
            , eto.ContentType
                , eto.Stream);

            await _minioService.UploadObjectAsync(obj);
        }
    }
}
