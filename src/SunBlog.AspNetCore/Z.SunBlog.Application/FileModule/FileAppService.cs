using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Z.EntityFrameworkCore.Extensions;
using Z.Fantasy.Core;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Minio;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.FileModule.Dto;
using Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.FileModule.FileManager;
using Z.SunBlog.Core.MinioFileModule.DomainManager;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.FileModule;

/// <summary>
/// 文件服务接口
/// </summary>
public interface IFileAppService : IApplicationService, ITransientDependency
{
    Task<List<UploadFileOutput>> UploadFile(IFormFile file);

    Task<IActionResult> GetFile(string fileUrl);

    Task<PageResult<FileInfoOutput>> GetPage([FromBody] FileInfoQueryInput input);

    Task DeleteAsync(KeyDto dto);
}

/// <summary>
/// 文件服务
/// </summary>
public class FileAppService : ApplicationService, IFileAppService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMinioFileManager _minioFileManager;
    private readonly MinioConfig _minioOptions;
    private readonly IFileInfoManager _fileInfoManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="webHostEnvironment"></param>
    /// <param name="minioFileManager"></param>
    /// <param name="minioOptions"></param>
    /// <param name="fileInfoManager"></param>
    public FileAppService(
        IServiceProvider serviceProvider,
        IWebHostEnvironment webHostEnvironment,
        IMinioFileManager minioFileManager,
        IOptions<MinioConfig> minioOptions
,
        IFileInfoManager fileInfoManager)
        : base(serviceProvider)
    {
        _webHostEnvironment = webHostEnvironment;
        _minioFileManager = minioFileManager;
        _minioOptions = minioOptions.Value;
        _fileInfoManager = fileInfoManager;
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
        //文件路径
        var minioname = $"{ZSunBlogConst.MinioAvatar}_{Guid.NewGuid().ToString("N")}/{file.FileName}";
        var url = await _fileInfoManager.UploadFileAsync(file, minioname);
        //await _minioFileManager.UploadMinio(file.OpenReadStream(), fileUrl, file.ContentType);
        return new List<UploadFileOutput>()
        {
            new()
            {
                Name = minioname,
                Url = url
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

    [HttpPost]
    public async Task<PageResult<FileInfoOutput>> GetPage([FromBody] FileInfoQueryInput input)
    {
        var filelist = await _fileInfoManager
                .QueryAsNoTracking.WhereIf(
                    !string.IsNullOrWhiteSpace(input.name),
                    x =>
                        x.FileDisplayName.Contains(input.name)
                        || x.FileName.Contains(input.name)
                        || x.FilePath.Contains(input.name)
                )
                .OrderByDescending(x => x.CreationTime)
                .Count(out var totalCount)
                .Page(input.PageNo, input.PageSize)
                .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)input.PageSize);
        return new PageResult<FileInfoOutput>()
        {
            PageNo = input.PageNo,
            PageSize = input.PageSize,
            Rows = ObjectMapper.Map<List<FileInfoOutput>>(filelist),
            Total = (int)totalCount,
            Pages = totalPages,
        };
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task DeleteAsync(KeyDto dto)
    {
       await _fileInfoManager.DeleteAsync(dto.Id);
    }
}
