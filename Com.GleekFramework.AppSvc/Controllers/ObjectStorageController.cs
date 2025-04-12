using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.ObjectSdk;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 对象存储控制器
    /// </summary>
    [Route("object_storage")]
    public class ObjectStorageController : BaseController
    {
        /// <summary>
        /// S3协议服务
        /// </summary>
        public S3ProtocolService S3ProtocolService { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">要上传的文件</param>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>上传结果</returns>
        [HttpPost("upload/stream")]
        public async Task<IActionResult> UploadFileAsync([Required] IFormFile file, [Required][FromForm] string fileKey, [FromForm] string bucketName = "")
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("文件不能为空");
            }

            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }
            var result = await S3ProtocolService.PutStreamAsync(fileKey, file, bucketName);
            if (result)
            {
                return Ok("文件上传成功");
            }
            else
            {
                return StatusCode(500, "文件上传失败");
            }
        }

        /// <summary>
        /// 上传文本内容
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>上传结果</returns>
        [HttpPost("upload/content")]
        public async Task<IActionResult> UploadContentAsync([Required][FromForm] string content, [Required][FromForm] string fileKey, [FromForm] string bucketName = "")
        {
            if (content == null || content.Length == 0)
            {
                return BadRequest("文本内容不能为空");
            }

            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }
            var result = await S3ProtocolService.PutContentAsync(fileKey, content, bucketName);
            if (result)
            {
                return Ok("文本内容上传成功");
            }
            else
            {
                return StatusCode(500, "文本内容上传失败");
            }
        }

        /// <summary>
        /// 上传对象
        /// </summary>
        /// <param name="param">要上传的文本内容</param>
        /// <returns>上传结果</returns>
        [HttpPost("upload/object")]
        public async Task<IActionResult> UploadContentAsync([Required] ComAreaModel param)
        {
            if (param == null)
            {
                return BadRequest("对象内容不能为空");
            }

            var fileKey = $"models/{Guid.NewGuid():N}.txt";
            var result = await S3ProtocolService.PutObjectAsync(fileKey, param);
            if (result)
            {
                return Ok(fileKey);
            }
            else
            {
                return StatusCode(500, fileKey);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>文件流</returns>
        [HttpGet("download/stream")]
        public async Task<IActionResult> DownloadFileAsync([Required][FromQuery] string fileKey, [FromQuery] string bucketName = "")
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }

            var fileStream = await S3ProtocolService.GetStreamAsync(fileKey, bucketName);
            if (fileStream == null)
            {
                return NotFound("文件不存在");
            }
            return File(fileStream, "application/octet-stream", Path.GetFileName(fileKey));
        }

        /// <summary>
        /// 下载文本内容
        /// </summary>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>文件流</returns>
        [HttpGet("download/content")]
        public async Task<IActionResult> GetContentAsync([Required][FromQuery] string fileKey, [FromQuery] string bucketName = "")
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }

            var content = await S3ProtocolService.GetContentAsync(fileKey, bucketName);
            return new ContentResult()
            {
                Content = content,
                ContentType = "text/plain"
            };
        }

        /// <summary>
        /// 下载对象
        /// </summary>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>文件流</returns>
        [HttpGet("download/object")]
        public async Task<IActionResult> GetObjectAsync([Required][FromQuery] string fileKey, [FromQuery] string bucketName = "")
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }

            var content = await S3ProtocolService.GetObjectAsync<ComAreaModel>(fileKey, bucketName);
            return new ContentResult()
            {
                Content = content.SerializeObject(),
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileKey">文件存储路径</param>
        /// <param name="bucketName">存储桶名称（可选）</param>
        /// <returns>文件流</returns>
        [HttpGet("delete")]
        public async Task<IActionResult> DeleteFileAsync([Required][FromQuery] string fileKey, [FromQuery] string bucketName = "")
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("文件存储路径不能为空");
            }

            var isSuccess = await S3ProtocolService.DeleteAsync(fileKey, bucketName);
            return new ContentResult()
            {
                ContentType = "text/plain",
                Content = isSuccess ? "文件删除成功" : "文件删除失败",
            };
        }
    }
}