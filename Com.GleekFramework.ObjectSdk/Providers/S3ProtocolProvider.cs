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
            // 构造函数是私有的，防止外部实例化
        }

        /// <summary>
        /// 创建AmazonS3客户端
        /// </summary>
        /// <returns> AmazonS3Client实例</returns>
        private static AmazonS3Client CreateClient() => new(Options.AccessKey, Options.SecretKey, new AmazonS3Config
        {
            ForcePathStyle = true,
            ServiceURL = Options.Endpoint,
        });

        /// <summary>
        /// 上传文本内容到S3存储桶
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="content">内容字符串</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public static async Task<bool> PutContentAsync(string fileKey, string content, string bucketName = "")
        {
            using var stream = content.ToStream();
            return await PutStreamAsync(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传对象到S3存储桶
        /// </summary>
        /// <typeparam name="T">要上传的对象类型，必须标记 [ProtoContract] 特性</typeparam>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="value">实例对象</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>上传是否成功</returns>
        public static async Task<bool> PutObjectAsync<T>(string fileKey, T value, string bucketName = "") where T : class
        {
            using var stream = value.SerializeObject();
            return await PutStreamAsync(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传文件到S3存储桶
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="file">要上传的文件</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>上传成功的文件路径</returns>
        public static async Task<bool> PutStreamAsync(string fileKey, IFormFile file, string bucketName = "")
        {
            using var stream = file.OpenReadStream();
            return await PutStreamAsync(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传文件到S3存储桶
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>上传成功的文件路径</returns>
        public static async Task<bool> PutStreamAsync(string fileKey, string filePath, string bucketName = "")
        {
            using var stream = filePath.OpenStream();
            return await PutStreamAsync(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传文件流到S3存储桶
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="stream">文件流</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>上传成功的文件路径</returns>
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
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName = "")
        {
            using var stream = await GetStreamAsync(fileKey, bucketName);
            return await stream.ToContentAsync();
        }

        /// <summary>
        /// 获取S3存储桶中对象的对象实例
        /// </summary>
        /// <typeparam name="T">要接收的对象类型，必须标记 [ProtoContract] 特性</typeparam>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string fileKey, string bucketName = "") where T : class
        {
            using var stream = await GetStreamAsync(fileKey, bucketName);
            return stream.DeserializeObject<T>();
        }

        /// <summary>
        /// 下载S3存储桶中的文件到本地
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public static async Task<bool> DownloadAsync(string fileKey, string filePath, string bucketName = "")
        {
            using var stream = await GetStreamAsync(fileKey, bucketName);
            using var fileStream = File.Create(filePath);
            await stream.CopyToAsync(fileStream);
            return true;
        }

        /// <summary>
        /// 获取S3存储桶中的文件流
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
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
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
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