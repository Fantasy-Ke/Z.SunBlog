namespace Z.Fantasy.Core.Minio;

public interface IMinioService
{
    /// <summary>
    /// 创建存储桶
    /// </summary>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task CreateBucketAsync(string bucketName);
    /// <summary>
    /// 删除存储桶
    /// </summary>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task RemoveBucket(string bucketName);
    /// <summary>
    /// 获取存储桶存储对象数据流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ObjectOutPut> GetObjectAsync(GetObjectInput input);
    /// <summary>
    /// 上传文件对象
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UploadObjectAsync(UploadObjectInput input);
    /// <summary>
    /// 上传本地文件使用默认存储桶
    /// </summary>
    /// <param name="uploads"></param>
    /// <returns></returns>
    Task UploadLocalUseDefaultBucket(List<UploadObjectInput> uploads);
    /// <summary>
    /// 创建默认存储桶
    /// </summary>
    /// <returns></returns>
    Task CreateDefaultBucket();
    /// <summary>
    /// 删除存储桶文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task RemoveObjectAsync(RemoveObjectInput input);
    /// <summary>
    /// 批量删除存储桶对象
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="bucketNames"></param>
    /// <returns></returns>
    Task BatchRemoveObjectAsync(string bucketName, List<string> objectNames);
}
