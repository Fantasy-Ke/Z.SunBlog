namespace Z.Fantasy.Core.Minio;

public class UploadObjectInput
{
    /// <summary>
    /// 命名空间
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 对象名称
    /// </summary>
    public string ObjectName { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 文件流
    /// </summary>
    public Stream Stream { get; set; }

    public UploadObjectInput() { }

    public UploadObjectInput(string bucketName, string objectName, string contentType, Stream stream)
    {
        BucketName = bucketName;
        ObjectName = objectName;
        ContentType = contentType;
        Stream = stream;
    }
}
