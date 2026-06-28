using Com.GleekFramework.AutofacSdk;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 阿里云OSS服务
    /// </summary>
    public partial class AliyunOSSService : IBaseAutofac
    {
        /// <summary>
        /// 上传文本内容到阿里云OSS
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="content">内容字符串</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public static async Task<bool> PutContentAsync(string fileKey, string content, string bucketName = "")
        {
            return await AliyunProvider.PutContentAsync(fileKey, content, bucketName);
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
            return await AliyunProvider.PutObjectAsync(fileKey, value, bucketName);
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
            return await AliyunProvider.PutStreamAsync(fileKey, file, bucketName);
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
            return await AliyunProvider.PutStreamAsync(fileKey, filePath, bucketName);
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
            return await AliyunProvider.PutStreamAsync(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 获取阿里云OSS中对象的内容
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName = "")
        {
            return await AliyunProvider.GetContentAsync(fileKey, bucketName);
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
            return await AliyunProvider.GetObjectAsync<T>(fileKey, bucketName);
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
            return await AliyunProvider.DownloadAsync(fileKey, filePath, bucketName);
        }

        /// <summary>
        /// 获取阿里云OSS中对象的文件流
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns>内容字符串</returns>
        public static async Task<Stream> GetStreamAsync(string fileKey, string bucketName = "")
        {
            return await AliyunProvider.GetStreamAsync(fileKey, bucketName);
        }

        /// <summary>
        /// 删除阿里云OSS中的对象
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public static async Task<bool> DeleteAsync(string fileKey, string bucketName = "")
        {
            return await AliyunProvider.DeleteAsync(fileKey, bucketName);
        }
    }
}