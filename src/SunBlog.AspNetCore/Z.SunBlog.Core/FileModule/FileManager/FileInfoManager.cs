﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Z.EventBus.EventBus;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Foundation.Core.Entities.Enum;
using Z.Foundation.Core.Entities.Files;
using Z.OSSCore;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.Handlers.FileHandlers;

namespace Z.SunBlog.Core.FileModule.FileManager
{
    public class FileInfoManager : BusinessDomainService<ZFileInfo>, IFileInfoManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILocalEventBus _localEvent;
        private readonly OSSOptions _ossOptions;

        public FileInfoManager(
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            ILocalEventBus localEvent,
            IOptions<OSSOptions> minioOptions
        )
            : base(serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _localEvent = localEvent;
            _ossOptions = minioOptions.Value;
        }

        public override Task ValidateOnCreateOrUpdate(ZFileInfo entity)
        {
            return Task.CompletedTask;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string objectName)
        {
            string extension = Path.GetExtension(file.FileName);
            // 文件完整名称
            var now = DateTime.Today;
            string filePath = GetTargetDirectory(file.ContentType, $"/{now.Year}-{now.Month:D2}/");
            var fileUrl = $"{filePath}{objectName}";
            var request = _httpContextAccessor.HttpContext!.Request;
            var fileinfo = new ZFileInfo(file.FileName,file.FileName.Replace(extension, ""),extension, file.ContentType, file.Length.ToString(),
                GetFileTypeFromContentType(file.ContentType), ZFileInfo.CreateCode(1));

            fileinfo.SetFileCode(await GetNextChildCodeAsync(fileinfo.ParentId));

            if (_ossOptions == null || !_ossOptions.Enable)
            {
                filePath = string.Concat(ZSunBlogConst.FileAvatar!.TrimEnd('/'), filePath);
                var webrootpath = _webHostEnvironment.WebRootPath;
                string s = Path.Combine(webrootpath, filePath).Replace("//","/");
                string fileDataPath = $"{s}{objectName}";
                var directoryPath = Path.GetDirectoryName(fileDataPath);
                if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                fileinfo.SetIpAddressOrPath($"{request.Scheme}://{request.Host.Value}",fileUrl);
                await CreateAsync(fileinfo);
                await using  var stream = File.Create(fileDataPath);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                string url = $"{request.Scheme}://{request.Host.Value}/{fileUrl}";
                return url;
            }
            var scheme = _ossOptions.IsEnableHttps ? "https" : "http";
            var fileUrlNew = string.Concat(_ossOptions.DefaultBucket!.TrimEnd('/'), fileUrl);
            fileinfo.SetIpAddressOrPath($"{scheme}://{_ossOptions.Endpoint!.TrimEnd('/')}",fileUrlNew);
            await _localEvent.PushAsync(
                new FileEventDto(file.OpenReadStream(), fileUrl, file.ContentType)
            );
            await CreateAsync(fileinfo);
            return $"{scheme}://{_ossOptions.Endpoint!.TrimEnd('/')}/{fileUrlNew}";
        }

        private static FileType GetFileTypeFromContentType(string contentType)
        {
            // 根据 Content-Type 判断文件类型
            // 这只是一个简单的示例，实际上可能需要更复杂的逻辑
            if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return FileType.Image;
            }
            else if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            {
                return FileType.Video;
            }
            else
            {
                return FileType.File;
            }
        }

        // 根据文件类型获取目标目录
        private static string GetTargetDirectory(string contentType, string filePath)
        {
            var fileType = GetFileTypeFromContentType(contentType);

            switch (fileType)
            {
                case FileType.Image:
                    return $"/Image{filePath}"; // 替换成实际的目录路径
                case FileType.Video:
                    return $"/Video{filePath}"; // 替换成实际的目录路径
                default:
                    return $"/File{filePath}"; // 替换成实际的目录路径
            }
        }

        protected virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return ZFileInfo.AppendCode(parentCode, ZFileInfo.CreateCode(1));
            }

            return ZFileInfo.CalculateNextCode(lastChild.Code);
        }

        /// <summary>
        ///     获取子集信息，可能为null
        /// </summary>
        /// <param name="parentId"> </param>
        /// <returns> </returns>
        protected virtual async Task<ZFileInfo> GetLastChildOrNullAsync(Guid? parentId)
        {
            return await QueryAsNoTracking.Where(x => x.ParentId == parentId)
                .OrderBy(s => s.Code).LastOrDefaultAsync();
        }

        /// <summary>
        ///     获取Code码
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        protected virtual async Task<string> GetCodeAsync(Guid id)
        {
            return (await FindByIdAsync(id)).Code;
        }
    }
}
