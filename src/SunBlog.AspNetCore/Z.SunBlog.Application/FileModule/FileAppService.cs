
using Azure.Core;
using Cuemon.Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Web;
using Z.Ddd.Common;
using Z.Ddd.Common.Attributes;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.Entities.Files;
using Z.Ddd.Common.Entities.Repositories;
using Z.Ddd.Common.Minio;
using Z.EventBus.EventBus;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.FileModule.Dto;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.Handlers.FileHandlers;
using Z.SunBlog.Core.MinioFileModule.DomainManager;

namespace Z.SunBlog.Application.FileModule;

public interface IFileAppService : IApplicationService, ITransientDependency
{
    Task<List<UploadFileOutput>> UploadFile(IFormFile file);

    Task<IActionResult> GetFile(string fileUrl);
}

public class FileAppService : ApplicationService, IFileAppService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMinioFileManager _minioFileManager;
    private readonly IBasicRepository<ZFileInfo> _fileRepository;
    private readonly ILocalEventBus _localEvent;
    private readonly MinioConfig _minioOptions;

    public FileAppService(IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IMinioFileManager minioFileManager,
         IOptions<MinioConfig> minioOptions,
         IBasicRepository<ZFileInfo> fileRepository,
         ILocalEventBus localEvent) : base(serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _minioFileManager = minioFileManager;
        _minioOptions = minioOptions.Value;
        _fileRepository = fileRepository;
        _localEvent = localEvent;
    }

    /// <summary>
    /// 上传附件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<List<UploadFileOutput>> UploadFile(IFormFile file)
    {
        if (file is null or { Length: 0 })
        {
            throw new Exception("请上传文件");
        }
        string extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new Exception("无效文件");
        }
        //文件路径

        var minioname = $"{ZSunBlogConst.MinioAvatar}_{Guid.NewGuid().ToString("N")}{extension}";
        // 文件完整名称
        var now = DateTime.Today;
        string filePath = $"/{now.Year}/{now.Month:D2}-{now.Day:D2}/";
        var fileUrl = $"{filePath}{minioname}";
        var request = _httpContextAccessor.HttpContext!.Request;
        var fileinfo = new ZFileInfo()
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            FilePath = string.Concat(_minioOptions.DefaultBucket!.TrimEnd('/'), filePath),
            FileSize = file.Length.ToString(),
            FileExt = extension,
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
            await _fileRepository.InsertAsync(fileinfo);
            var stream = System.IO.File.Create($"{s}{minioname}");
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
            fileUrl = string.Concat(_minioOptions.DefaultBucket.TrimEnd('/'), fileUrl);
            string url = $"{request.Scheme}://{request.Host.Value}/{fileUrl}";
            return new List<UploadFileOutput>()
            {
                new()
                {
                    Name = minioname,
                    Url = url
                }
            };
        }
        await _fileRepository.InsertAsync(fileinfo);
        await _localEvent.PushAsync(new FileEventDto(file.OpenReadStream(), fileUrl, file.ContentType));
        //await _minioFileManager.UploadMinio(file.OpenReadStream(), fileUrl, file.ContentType);
        return new List<UploadFileOutput>()
        {
            new()
            {
                Name = minioname,
                Url = $"{request.Scheme}://{_minioOptions.Host!.TrimEnd('/')}/{string.Concat(_minioOptions.DefaultBucket!.TrimEnd('/'), fileUrl)}"
            }
        };
    }


    /// <summary>
    /// 获取文件
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <returns></returns>
    public async Task<IActionResult> GetFile(string fileUrl)
    {
        //是否开启minio
        if (!_minioOptions.Enable)
        {
            var webrootpath = _webHostEnvironment.WebRootPath;
            string s = Path.Combine(webrootpath, fileUrl);
            var contentType = MimeTypes.GetMimeType(fileUrl);
            var stream = System.IO.File.OpenRead(s);
            return new FileStreamResult(stream, contentType);
        }
        fileUrl = fileUrl.Replace(_minioOptions.DefaultBucket!.TrimEnd('/'), "");
        var output = await _minioFileManager.GetFile(fileUrl);

        return new FileStreamResult(output.Stream, output.ContentType);
    }
}