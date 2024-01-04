using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Z.EventBus.EventBus;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Entities.EntityLog;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Entities.Files;
using Z.Fantasy.Core.Minio;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.Handlers.FileHandlers;

namespace Z.SunBlog.Core.FileModule.FileManager
{
    public class FileInfoManager : BusinessDomainService<ZFileInfo>, IFileInfoManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILocalEventBus _localEvent;
        private readonly MinioConfig _minioOptions;

        public FileInfoManager(
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            ILocalEventBus localEvent,
            IOptions<MinioConfig> minioOptions
        )
            : base(serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _localEvent = localEvent;
            _minioOptions = minioOptions.Value;
        }

        public override Task ValidateOnCreateOrUpdate(ZFileInfo entity)
        {
            return Task.CompletedTask;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string minioName)
        {
            string extension = Path.GetExtension(file.FileName);
            // 文件完整名称
            var now = DateTime.Today;
            string filePath = GetTargetDirectory(file.ContentType, $"/{now.Year}-{now.Month:D2}/");
            var fileUrl = $"{filePath}{minioName}";
            var request = _httpContextAccessor.HttpContext!.Request;
            var fileinfo = new ZFileInfo()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FilePath = string.Concat(_minioOptions.DefaultBucket!.TrimEnd('/'), filePath),
                FileSize = file.Length.ToString(),
                FileExt = extension,
                FileType = GetFileTypeFromContentType(file.ContentType),
                FileDisplayName = file.FileName.Replace(extension, ""),
                Code = ZFileInfo.CreateCode(1)
            };

            if (!_minioOptions.Enable)
            {
                filePath = string.Concat(_minioOptions.DefaultBucket!.TrimEnd('/'), filePath);
                var webrootpath = _webHostEnvironment.WebRootPath;
                string s = Path.Combine(webrootpath, filePath);
                if (!Directory.Exists(s))
                {
                    Directory.CreateDirectory(s);
                }
                await CreateAsync(fileinfo);
                var stream = System.IO.File.Create($"{s}{minioName}");
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                fileUrl = string.Concat(_minioOptions.DefaultBucket.TrimEnd('/'), fileUrl);
                string url = $"{request.Scheme}://{request.Host.Value}/{fileUrl}";
                return url;
            }
            await CreateAsync(fileinfo);
            await _localEvent.PushAsync(
                new FileEventDto(file.OpenReadStream(), fileUrl, file.ContentType)
            );
            return $"{request.Scheme}://{_minioOptions.Host!.TrimEnd('/')}/{string.Concat(_minioOptions.DefaultBucket!.TrimEnd('/'), fileUrl)}";
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
    }
}
