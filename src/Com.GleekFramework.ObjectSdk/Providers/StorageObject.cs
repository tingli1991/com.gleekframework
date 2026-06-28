using Com.GleekFramework.NLogSdk;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 对象存储提供程序基类，封装公共逻辑
    /// </summary>
    public class StorageObject
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected static ObjectStorageOptions Options => ObjectStorageOptions.Instance;

        /// <summary>
        /// 处理包装器
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="func">要执行的异步方法</param>
        /// <returns>异步方法的执行结果</returns>
        protected static async Task<T> ExecuteAsync<T>(string bucketName, Func<string, Task<T>> func)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bucketName))
                {
                    bucketName = Options.DefaultBucketName;
                }
                return await func(bucketName);
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【对象存储】操作失败：{ex}");
                throw;
            }
        }

        /// <summary>
        /// 上传文本内容
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="content">内容字符串</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="putStreamFunc">上传流操作</param>
        /// <returns></returns>
        public static async Task<bool> PutContentAsync(string fileKey, string content, string bucketName, Func<string, Stream, string, Task<bool>> putStreamFunc)
        {
            using var stream = content.ToStream();
            return await putStreamFunc(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传对象
        /// </summary>
        /// <typeparam name="T">要上传的对象类型，必须标记 [ProtoContract] 特性</typeparam>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="value">实例对象</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="putStreamFunc">上传流操作</param>
        /// <returns>上传是否成功</returns>
        public static async Task<bool> PutObjectAsync<T>(string fileKey, T value, string bucketName, Func<string, Stream, string, Task<bool>> putStreamFunc) where T : class
        {
            using var stream = value.SerializeObject();
            return await putStreamFunc(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="file">要上传的文件</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="putStreamFunc">上传流操作</param>
        /// <returns>上传成功的文件路径</returns>
        public static async Task<bool> PutStreamAsync(string fileKey, IFormFile file, string bucketName, Func<string, Stream, string, Task<bool>> putStreamFunc)
        {
            using var stream = file.OpenReadStream();
            return await putStreamFunc(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="putStreamFunc">上传流操作</param>
        /// <returns>上传成功的文件路径</returns>
        public static async Task<bool> PutStreamAsync(string fileKey, string filePath, string bucketName, Func<string, Stream, string, Task<bool>> putStreamFunc)
        {
            using var stream = filePath.OpenStream();
            return await putStreamFunc(fileKey, stream, bucketName);
        }

        /// <summary>
        /// 获取对象的内容
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="getStreamFunc">获取流操作</param>
        /// <returns>内容字符串</returns>
        public static async Task<string> GetContentAsync(string fileKey, string bucketName, Func<string, string, Task<Stream>> getStreamFunc)
        {
            using var stream = await getStreamFunc(fileKey, bucketName);
            return await stream.ToContentAsync();
        }

        /// <summary>
        /// 获取对象的对象实例
        /// </summary>
        /// <typeparam name="T">要接收的对象类型，必须标记 [ProtoContract] 特性</typeparam>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="getStreamFunc">获取流操作</param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string fileKey, string bucketName, Func<string, string, Task<Stream>> getStreamFunc) where T : class
        {
            using var stream = await getStreamFunc(fileKey, bucketName);
            return stream.DeserializeObject<T>();
        }

        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="fileKey">桶的存放路径</param>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="getStreamFunc">获取流操作</param>
        /// <returns></returns>
        public static async Task<bool> DownloadAsync(string fileKey, string filePath, string bucketName, Func<string, string, Task<Stream>> getStreamFunc)
        {
            using var stream = await getStreamFunc(fileKey, bucketName);
            using var fileStream = File.Create(filePath);
            await stream.CopyToAsync(fileStream);
            return true;
        }
    }
}
