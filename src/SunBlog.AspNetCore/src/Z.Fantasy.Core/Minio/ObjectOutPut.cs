namespace Z.Fantasy.Core.Minio;

public class ObjectOutPut
{
    /// <summary>
    /// 文件名--对象名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 文件流
    /// </summary>
    public Stream Stream { get; set; }
    /// <summary>
    /// 上传文件格式
    /// </summary>
    public string ContentType { get; set; }

    public ObjectOutPut() { }

    public ObjectOutPut(string name, Stream stream, string contentType)
    {
        Name = name;
        Stream = stream;
        ContentType = contentType;
    }

}
