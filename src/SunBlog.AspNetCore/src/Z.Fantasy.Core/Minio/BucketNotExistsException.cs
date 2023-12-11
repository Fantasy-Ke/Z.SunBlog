namespace Z.Fantasy.Core.Minio;

public class BucketExistsException : Exception
{
    public BucketExistsException() { }

    public BucketExistsException(string message) : base(message) { }
}

public class BucketNotExistsException : Exception
{
    public BucketNotExistsException() { }

    public BucketNotExistsException(string message) : base(message) { }
}

public static class ThrowBucketNotExistisException
{
    public static void ExistsException(string bucketName)
    {
        throw new BucketExistsException($"{bucketName}已存在");
    }

    public static void NotExistsException(string bucketName)
    {
        throw new BucketNotExistsException($"{bucketName}不存在");
    }
}
