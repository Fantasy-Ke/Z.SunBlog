namespace Z.Fantasy.Core.Minio;

public class GetObjectInput
{
    /// <summary>
    /// 命名空间
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 对象名称
    /// </summary>
    public string ObjectName { get; set; }
}
