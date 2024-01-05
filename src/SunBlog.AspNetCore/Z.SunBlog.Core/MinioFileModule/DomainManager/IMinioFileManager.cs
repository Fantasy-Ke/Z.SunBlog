using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Minio;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Core.MinioFileModule.DomainManager
{
    public interface IMinioFileManager : IDomainService, ITransientDependency
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="file"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task UploadMinio(Stream stream, string file, string contentType);


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="file"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task DeleteMinioFileAsync(RemoveObjectInput input);


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<ObjectOutPut> GetFile(string filename);
    }
}
