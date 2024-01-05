using Microsoft.Extensions.Options;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Minio;

namespace Z.SunBlog.Core.MinioFileModule.DomainManager
{
    public class MinioFileManager : DomainService, IMinioFileManager
    {
        private readonly IMinioService _minioService;
        private readonly MinioConfig _minioOptions;
        public MinioFileManager(IServiceProvider serviceProvider, IMinioService minioService, IOptions<MinioConfig> minioOptions) : base(serviceProvider)
        {
            _minioService = minioService;
            _minioOptions = minioOptions.Value;
        }

        public async Task DeleteMinioFileAsync(RemoveObjectInput input)
        {
            await _minioService.RemoveObjectAsync(input);
        }

        public async Task<ObjectOutPut> GetFile(string fileUrl)
        {
            var obj = new GetObjectInput()
            {
                ObjectName = fileUrl,
                BucketName = _minioOptions.DefaultBucket
            };

            return await _minioService.GetObjectAsync(obj);
        }

        public async Task UploadMinio(Stream stream, string file, string contentType)
        {
            var obj = new UploadObjectInput(_minioOptions.DefaultBucket
                , file
            , contentType
                , stream);

            await _minioService.UploadObjectAsync(obj);
        }
    }
}
