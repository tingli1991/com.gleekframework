using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// Http上下文拓展类
    /// </summary>
    public static partial class ContextExtensions
    {
        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<string> GetResponseBody(this HttpResponse response)
        {
            var text = "";
            if (response == null)
            {
                return text;
            }

            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body);
            text = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        public static async Task<string> GetRequestBody(this HttpRequest request)
        {
            if (request == null)
            {
                return "";
            }

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            if (request.ContentType == "application/x-www-form-urlencoded")
            {
                body = WebUtility.UrlDecode(body);
            }
            request.Body.Position = 0;
            return body;
        }
    }
}