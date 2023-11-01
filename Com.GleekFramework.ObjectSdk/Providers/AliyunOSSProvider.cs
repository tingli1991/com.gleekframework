using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// OSS实现类
    /// </summary>
    public static class AliyunOSSProvider
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public static AliyunOSSOptions Options { get; set; }

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
            if (files == null || !files.Any())
            {
                return fileNames;
            }

            foreach (var file in files)
            {
                var fileName = await PutObjectAsync(bucketName, filePath, file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileNames.Add(fileName);
                }
            }
            return fileNames;
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
            var fileName = $"{Path.GetFileName(file.FileName)}";
            using var inputStream = file.OpenReadStream();
            return await PutObjectAsync(bucketName, filePath, fileName, inputStream);
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
            var fileName = Path.GetFileName(fullName);//文件扩展名，格式"xxx.txt"
            using Stream stream = new FileStream(fullName, FileMode.Open);
            var responseFileName = await PutObjectAsync(bucketName, filePath, fileName, stream);
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
            return responseFileName;
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
            fileName = $"{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
            var client = new OssClient(Options.BaseUrl, Options.AppId, Options.AppKey);
            var response = client.PutObject(bucketName, $"{filePath}/{fileName}", stream);
            if (response == null)
            {
                //上传失败，不返回连接
                return "";
            }
            return await Task.FromResult($"{Options.BucketUrl.TrimEnd('/')}/{filePath.TrimStart('/').TrimEnd('/')}/{fileName}");
        }
    }
}