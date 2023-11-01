using Com.GleekFramework.AutofacSdk;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
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
        /// 上传文件到OSS
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="files">文件列表</param>
        /// <returns></returns>
        public static async Task<List<string>> PutObjectAsync(string bucketName, string filePath, IEnumerable<IFormFile> files)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, filePath, files);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string filePath, IFormFile file)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, filePath, file);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="filePath"></param>
        /// <param name="fullName">文件名称(格式：d://xxx.jpg)</param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string filePath, string fullName)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, filePath, fullName);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="filePath">文件路径(xxx/sdsss)</param>
        /// <param name="fileName">文件名称(格式：xxx.jpg)</param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string filePath, string fileName, Stream stream)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, filePath, fileName, stream);
        }
    }
}