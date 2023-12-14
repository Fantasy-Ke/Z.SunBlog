using Minio;
using Microsoft.Extensions.Options;
using Minio.DataModel;
using System.Security.Cryptography;
using Z.Module.DependencyInjection;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Serilog;
using System.Diagnostics;

namespace Z.Fantasy.Core.Minio
{
    public class MinioService : IMinioService
    {
        private readonly MinioClient _minioClient;

        private readonly IOptions<MinioConfig> _minioOptions;

        public MinioService(MinioClient minioClient
            , IOptions<MinioConfig> minioOptions)
        {
            _minioClient = minioClient;
            _minioOptions = minioOptions;
        }

        /// <summary>
        /// 当存储桶为空时使用默认存储桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void SetDefaultBucket(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                bucketName = _minioOptions.Value?.DefaultBucket ?? throw new ArgumentNullException("Minio基础配置默认存储桶为空");
            }
        }
        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task CreateBucketAsync(string bucketName)
        {
            ///设置存储桶
            SetDefaultBucket(bucketName);

            var bucketArgs = new BucketExistsArgs();
            bucketArgs.WithBucket(bucketName);

            if (!await _minioClient.BucketExistsAsync(bucketArgs))
            {
                ThrowBucketNotExistisException.ExistsException(bucketName);
            }

            var makeArgs = new MakeBucketArgs();
            makeArgs.WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(makeArgs);
        }
        /// <summary>
        /// 删除存储桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task RemoveBucket(string bucketName)
        {
            SetDefaultBucket(bucketName);

            var bucketArgs = new BucketExistsArgs();
            bucketArgs.WithBucket(bucketName);

            if (!await _minioClient.BucketExistsAsync(bucketArgs))
            {
                ThrowBucketNotExistisException.NotExistsException(bucketName);
            }

            var removeArgs = new RemoveBucketArgs();
            removeArgs.WithBucket(bucketName);

            await _minioClient.RemoveBucketAsync(removeArgs);
        }
        /// <summary>
        /// 获取存储桶存储对象数据流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ObjectOutPut> GetObjectAsync(GetObjectInput input)
        {
            SetDefaultBucket(input.BucketName);

            StatObjectArgs statObjectArgs = new StatObjectArgs()
                                    .WithBucket(input.BucketName)
                                    .WithObject(input.ObjectName);

            await _minioClient.StatObjectAsync(statObjectArgs);

            MemoryStream objStream = new MemoryStream();


            GetObjectArgs getObjectArgs = new GetObjectArgs()
                      .WithBucket(input.BucketName)
                      .WithObject(input.ObjectName)
                      .WithCallbackStream((stream) =>
                      {
                          //stream.CopyTo(Console.OpenStandardOutput());
                          if (stream is null)
                          {
                              throw new ArgumentNullException("Minio文件对象流为空");
                          }
                          //stream.CopyTo(objStream);
                          stream.CopyTo(objStream);
                          objStream.Position = 0;
                      });
            objStream.Position = 0;

            var statObj = await _minioClient.GetObjectAsync(getObjectArgs);

            return new ObjectOutPut(statObj.ObjectName
                , objStream
                , statObj.ContentType);
        }
        /// <summary>
        /// 上传文件对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UploadObjectAsync(UploadObjectInput input)
        {
            SetDefaultBucket(input.BucketName);

            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(input.BucketName));
            //如果桶不存在则创建桶
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(input.BucketName));
            }

            //ObjectStat stat = null;

            //try
            //{
            //    StatObjectArgs statObjectArgs = new StatObjectArgs()
            //                        .WithBucket(input.BucketName)
            //                        .WithObject(input.ObjectName);
            //    stat = await _minioClient.StatObjectAsync(statObjectArgs);
            //}
            //catch (Exception ex)
            //{
            //    Log.Warning($"{ex.Message}");
            //}

            //if (stat != null)
            //{
            //    ThrowMinioFileExistsException.FileExistsException(input.ObjectName);
            //}

            PutObjectArgs putObjectArgs = new PutObjectArgs()
                                              .WithBucket(input.BucketName)
                                              .WithObject(input.ObjectName)
                                              .WithStreamData(input.Stream)
                                              .WithContentType(input.ContentType)
                                              .WithObjectSize(input.Stream.Length);
            //.WithServerSideEncryption(ssec);

            if (_minioOptions.Value.Encryption)
            {
                Aes aesEncryption = Aes.Create();
                aesEncryption.KeySize = 256;
                aesEncryption.GenerateKey();

                var ssec = new SSEC(aesEncryption.Key);
                putObjectArgs.WithServerSideEncryption(ssec);
            }

            await _minioClient.PutObjectAsync(putObjectArgs);
        }
        /// <summary>
        /// 上传本地文件使用默认存储桶
        /// </summary>
        /// <param name="uploads"></param>
        /// <returns></returns>
        public async Task UploadLocalUseDefaultBucket(List<UploadObjectInput> uploads)
        {
            List<Task> tasks = new List<Task>();

            foreach (var item in uploads)
            {
                var task = UploadObjectAsync(item);

                if (tasks.Count > 25)
                {
                    await Task.WhenAll(tasks);

                    tasks.Clear();
                }
            }
        }
        /// <summary>
        /// 创建默认存储桶
        /// </summary>
        /// <returns></returns>
        public async Task CreateDefaultBucket()
        {
            var config = _minioOptions.Value;

            var defaultBucket = config.DefaultBucket;
            ///创建默认存储桶
            if (string.IsNullOrEmpty(config.DefaultBucket))
            {
                return;
            }
            var bucketArgs = new BucketExistsArgs();
            bucketArgs.WithBucket(config.DefaultBucket);

            if (await _minioClient.BucketExistsAsync(bucketArgs))
            {
                return;
            }

            var makeArgs = new MakeBucketArgs();
            makeArgs.WithBucket(config.DefaultBucket);

            await _minioClient.MakeBucketAsync(makeArgs);
        }
        /// <summary>
        /// 删除存储桶文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task RemoveObjectAsync(RemoveObjectInput input)
        {
            SetDefaultBucket(input.BucketName);

            ObjectStat stat = null;

            try
            {
                StatObjectArgs statObjectArgs = new StatObjectArgs()
                                    .WithBucket(input.BucketName)
                                    .WithObject(input.ObjectName);
                stat = await _minioClient.StatObjectAsync(statObjectArgs);
            }
            catch (Exception ex)
            {
                Log.Warning($"{DateTime.Now}:{ex.Message}--{ex.ToString()}");

                ThrowMinioFileExistsException.FileNotExistsException(input.ObjectName);
            }

            RemoveObjectArgs rmArgs = new RemoveObjectArgs()
                              .WithBucket(input.BucketName)
                              .WithObject(input.ObjectName);

            await _minioClient.RemoveObjectAsync(rmArgs);
        }
        /// <summary>
        /// 批量删除存储桶对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="bucketNames"></param>
        /// <returns></returns>
        public async Task BatchRemoveObjectAsync(string bucketName, List<string> objectNames)
        {
            SetDefaultBucket(bucketName);

            RemoveObjectsArgs rmArgs = new RemoveObjectsArgs()
                                .WithBucket(bucketName)
                                .WithObjects(objectNames);

            var observable = await _minioClient.RemoveObjectsAsync(rmArgs);

            observable.Subscribe(item =>
            {
                Log.Warning($"{item.Key}文件对象删除失败");
            }, ex =>
            {
                Log.Warning($"{DateTime.Now}:{ex.Message}-{ex.ToString()}");
            }, () =>
            {
                Log.Information("批量删除文件对象成功");
            });
        }
    }
}
