using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 阿里云对象存储实现类
    /// </summary>
    public class AliyunProvider : StorageObject
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private AliyunProvider()
        {
        }

        /// <summary>
        /// 创建阿里云OSS客户端
        /// </summary>
        /// <returns>OssClient 实例</returns>
        private static OssClient CreateClient() => new(Options.Endpoint, Options.AccessKey, Options.SecretKey);

        /// <summary>
        /// 上传文本内容到阿里云OSS
        /// </summary>
        public static async Task<bool> PutContentAsync(string fileKey, string content, string bucketName = "")
            => await PutContentAsync(fileKey, content, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传对象到阿里云OSS
        /// </summary>
        public static async Task<bool> PutObjectAsync<T>(string fileKey, T value, string bucketName = "") where T : class
            => await PutObjectAsync(fileKey, value, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件到阿里云OSS
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, IFormFile file, string bucketName = "")
            => await PutStreamAsync(fileKey, file, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件到阿里云OSS
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, string filePath, string bucketName = "")
            => await PutStreamAsync(fileKey, filePath, bucketName, PutStreamAsync);

        /// <summary>
        /// 上传文件流到阿里云OSS
        /// </summary>
        public static async Task<bool> PutStreamAsync(string fileKey, Stream stream, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                var client = CreateClient();
                var result = client.PutObject(newBucketName, fileKey, stream);
                return await Task.FromResult(result != null);
            });
        }

        /// <summary>
        /// 获取阿里云OSS中对象的内容
        /// </summary>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName = "")
            => await GetContentAsync(fileKey, bucketName, GetStreamAsync);

        /// <summary>
        /// 获取阿里云OSS中对象的对象实例
        /// </summary>
        public static async Task<T> GetObjectAsync<T>(string fileKey, string bucketName = "") where T : class
            => await GetObjectAsync<T>(fileKey, bucketName, GetStreamAsync);

        /// <summary>
        /// 下载阿里云OSS中的文件到本地
        /// </summary>
        public static async Task<bool> DownloadAsync(string fileKey, string filePath, string bucketName = "")
            => await DownloadAsync(fileKey, filePath, bucketName, GetStreamAsync);

        /// <summary>
        /// 获取阿里云OSS中对象的文件流
        /// </summary>
        public static async Task<Stream> GetStreamAsync(string fileKey, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                var client = CreateClient();
                var response = client.GetObject(newBucketName, fileKey);
                return await Task.FromResult(response.Content);
            });
        }

        /// <summary>
        /// 删除阿里云OSS中的对象
        /// </summary>
        public static async Task<bool> DeleteAsync(string fileKey, string bucketName = "")
        {
            return await ExecuteAsync(bucketName, async (newBucketName) =>
            {
                var client = CreateClient();
                client.DeleteObject(newBucketName, fileKey);
                return await Task.FromResult(true);
            });
        }
    }
}
