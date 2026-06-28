using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 基于S3协议的对象存储实现类
    /// </summary>
    public class S3ProtocolProvider : StorageObject
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private S3ProtocolProvider()
        {
        }

        /// <summary>
        /// 创建AmazonS3客户端
        /// </summary>
        private static AmazonS3Client CreateClient() => new(Options.AccessKey, Options.SecretKey, new AmazonS3Config
        {
            ForcePathStyle = true,
            ServiceURL = Options.Endpoint,
        });

        /// <summary>
        /// 上传文本内容到S3存储桶
        /// </summary>
        public static async Task<bool> PutContentAsync(string fileKey, string content, string bucketName = "")
            => await PutContentAsync(fileKey, content, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传对象到S3存储桶
        /// </summary>
        public static async Task<bool> PutObjectAsync<T>(string fileKey, T value, string bucketName = "") where T : class
            => await PutObjectAsync(fileKey, value, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件到S3存储桶
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, IFormFile file, string bucketName = "")
            => await PutStreamAsync(fileKey, file, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件到S3存储桶
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, string filePath, string bucketName = "")
            => await PutStreamAsync(fileKey, filePath, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件流到S3存储桶
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, Stream stream, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                using var client = CreateClient();
                var request = new PutObjectRequest()
                {
                    Key = fileKey,
                    InputStream = stream,
                    BucketName = newBucketName,
                };
                var response = await client.PutObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            });
        }

        /// <summary>
        /// 获取S3存储桶中对象的文本内容
        /// </summary>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName = "")
            => await GetContentAsync(fileKey, bucketName, GetStreamAsync);

        /// <summary>
        /// 获取S3存储桶中对象的对象实例
        /// </summary>
        public static async Task<T> GetObjectAsync<T>(string fileKey, string bucketName = "") where T : class
            => await GetObjectAsync<T>(fileKey, bucketName, GetStreamAsync);

        /// <summary>
        /// 下载S3存储桶中的文件到本地
        /// </summary>
        public static async Task<bool> DownloadAsync(string fileKey, string filePath, string bucketName = "")
            => await DownloadAsync(fileKey, filePath, bucketName, GetStreamAsync);

        /// <summary>
        /// 获取S3存储桶中的文件流
        /// </summary>
        public static async Task<Stream> GetStreamAsync(string fileKey, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                using var client = CreateClient();
                var request = new GetObjectRequest()
                {
                    Key = fileKey,
                    BucketName = newBucketName,
                };
                var response = await client.GetObjectAsync(request);
                return response.ResponseStream;
            });
        }

        /// <summary>
        /// 删除S3存储桶中的对象
        /// </summary>
        public static async Task<bool> DeleteAsync(string fileKey, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                using var client = CreateClient();
                var request = new DeleteObjectRequest()
                {
                    Key = fileKey,
                    BucketName = newBucketName,
                };
                var response = await client.DeleteObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            });
        }
    }
}
