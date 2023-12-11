namespace Z.Fantasy.Core.Minio;

public class MinioFileExistsException : Exception
{
    public MinioFileExistsException() { }

    public MinioFileExistsException(string message) : base(message)
    {

    }
}

public class MinioFileNotExistsException : Exception
{
    public MinioFileNotExistsException() { }

    public MinioFileNotExistsException(string message) : base(message) { }
}


public static class ThrowMinioFileExistsException
{
    public static void FileExistsException(string objectName)
    {
        throw new MinioFileExistsException($"{objectName}文件存在当前存储桶");
    }

    public static void FileNotExistsException(string objectName)
    {
        throw new MinioFileNotExistsException($"{objectName}文件不存在当前存储桶");
    }
}
