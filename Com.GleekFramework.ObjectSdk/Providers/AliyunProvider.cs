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
            // 构造函数是私有的，防止外部实例化
        }

        /// <summary>
        /// 创建阿里云OSS客户端
        /// </summary>
        /// <returns>OssClient 实例</returns>
        private static OssClient CreateClient() => new(Options.Endpoint, Options.AccessKey, Options.SecretKey);

        /// <summary>
        /// 上传文本内容到阿里云OSS
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
        /// 上传对象到阿里云OSS
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
        /// 上传文件到阿里云OSS
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
        /// 上传文件到阿里云OSS
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
        /// 上传文件流到阿里云OSS
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="stream">文件流</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>上传成功的文件路径</returns>
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
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName = "")
        {
            using var stream = await GetStreamAsync(fileKey, bucketName);
            return await stream.ToContentAsync();
        }

        /// <summary>
        /// 获取阿里云OSS中对象的对象实例
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
        /// 下载阿里云OSS中的文件到本地
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
        /// 获取阿里云OSS中对象的文件流
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
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
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
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
