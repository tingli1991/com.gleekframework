using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
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
            var fileNames = new List<string>();
            foreach (var file in files)
            {
                var fileName = await PutObjectAsync(bucketName, filePath, file);
                if (!fileName.IsNullOrEmpty())
                {
                    fileNames.Add(fileName);
                }
            }
            return fileNames;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="path">文件存放路径</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string path, IFormFile file)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, path, file);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="path">文件存放路径</param>
        /// <param name="fullName">文件名称(格式：d://xxx.jpg)</param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string path, string fullName)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, path, fullName);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="fileName">文件名称(格式：sss/xxx.jpg)</param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string fileName, Stream stream)
        {
            return await AliyunOSSProvider.PutObjectAsync(bucketName, fileName, stream);
        }

        /// <summary>
        /// 上传支付穿
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">字符串内容</param>
        /// <returns></returns>
        public static async Task<string> PutContentAsync(string bucketName, string fileName, string content)
        {
            return await AliyunOSSProvider.PutContentAsync(bucketName, fileName, content);
        }

        /// <summary>
        /// 获取对象的内容
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="fileName">文件名称(格式：sss/xxx.jpg)</param>
        /// <returns></returns>
        public static async Task<string> GetContentAsync(string bucketName, string fileName)
        {
            return await AliyunOSSProvider.GetContentAsync(bucketName, fileName);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="path">文件存放路径(格式：d:/www)</param>
        /// <param name="fileName">要下载的文件</param>
        /// <returns></returns>
        public static async Task<string> Download(string bucketName, string path, string fileName)
        {
            return await AliyunOSSProvider.Download(bucketName, path, fileName);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="path">文件存放路径(格式：d:/www)</param>
        /// <param name="fileNames">要下载的文件集合</param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> Download(string bucketName, string path, IEnumerable<string> fileNames)
        {
            return await AliyunOSSProvider.Download(bucketName, path, fileNames);
        }
    }
}