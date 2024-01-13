using Aliyun.OSS;
using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            if (files.IsNullOrEmpty())
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
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="path">文件存放路径</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> PutObjectAsync(string bucketName, string path, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var fileName = $"{path.TrimStart('/').TrimEnd('/')}/{Path.GetFileName(file.FileName)}";
            return await PutObjectAsync(bucketName, fileName, stream);
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
            using Stream stream = new FileStream(fullName, FileMode.Open);
            var fileName = $"{path.TrimStart('/').TrimEnd('/')}/{Path.GetFileName(fullName)}";
            var responseFileName = await PutObjectAsync(bucketName, fileName, stream);
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
            return responseFileName;
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
            fileName = fileName.TrimStart('/');//去掉前面的斜杠
            var client = new OssClient(Options.Endpoint, Options.AccessKey, Options.AccessSecret);
            var response = client.PutObject(bucketName, fileName, stream);
            if (response == null)
            {
                //上传失败，不返回连接
                return "";
            }
            return await Task.FromResult($"{Options.BucketUrl.TrimEnd('/')}/{fileName}");
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
            var bitys = Encoding.UTF8.GetBytes(content);
            using MemoryStream stream = new MemoryStream(bitys);
            return await PutObjectAsync(bucketName, fileName, stream);
        }

        /// <summary>
        /// 获取对象的内容
        /// </summary>
        /// <param name="bucketName">Bucket名称</param>
        /// <param name="fileName">文件名称(格式：sss/xxx.jpg)</param>
        /// <returns></returns>
        public static async Task<string> GetContentAsync(string bucketName, string fileName)
        {
            var client = new OssClient(Options.Endpoint, Options.AccessKey, Options.AccessSecret);
            var response = client.GetObject(bucketName, fileName);
            if (response == null)
            {
                //返回空字符串
                return "";
            }

            using var memoryStream = new MemoryStream();
            response.Content.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();
            var objectContent = Encoding.UTF8.GetString(bytes);
            response.Content.Close();
            return await Task.FromResult(objectContent);
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
            var fileNames = await Download(bucketName, path, new string[] { fileName });
            return fileNames.FirstOrDefault();
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
            var directoryPath = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryPath))
            {
                //创建目录
                Directory.CreateDirectory(directoryPath);
            }

            var successFileNames = new List<string>();
            var client = new OssClient(Options.Endpoint, Options.AccessKey, Options.AccessSecret);
            foreach (var fileName in fileNames)
            {
                var response = client.GetObject(bucketName, fileName);
                if (response == null)
                {
                    //返回空字符串
                    continue;
                }
                var filePath = Path.Combine(directoryPath, fileName);
                using var fileStream = File.Create(filePath);
                response.Content.CopyTo(fileStream);
                successFileNames.Add(fileName);
            }
            return await Task.FromResult(successFileNames);
        }
    }
}