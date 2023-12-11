namespace Z.Fantasy.Core.Minio;

public class RemoveObjectInput
{
    /// <summary>
    /// 存储桶名称
    /// </summary>
    public string BucketName { get; set; }
    /// <summary>
    /// 存储对象名称
    /// </summary>
    public string ObjectName { get; set; }
}
