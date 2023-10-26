using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Http客户端扩展
    /// </summary>
    internal static partial class HttpClientExtensions
    {
        /// <summary>
        /// http客户端
        /// </summary>
        public static readonly HttpClient client;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static HttpClientExtensions()
        {
            client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        }

        /// <summary>
        /// Get接口请求
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(this Dictionary<string, string> paramters, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            var requestUri = url.ToGetUrl(paramters);
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                //请求状态不对的时候抛出异常信息
                throw new HttpRequestException($"StatusCode：{(int)response.StatusCode}，ReasonPhrase：{response.ReasonPhrase}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 删除请求
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DeleteAsync(this Dictionary<string, string> paramters, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            var requestUri = url.ToGetUrl(paramters);
            var response = await client.DeleteAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                //请求状态不对的时候抛出异常信息
                throw new HttpRequestException($"StatusCode：{(int)response.StatusCode}，ReasonPhrase：{response.ReasonPhrase}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// PUT请求
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> PutAsync(this Dictionary<string, string> paramters, string url, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            var requestUri = url.ToGetUrl(paramters);
            var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    httpContent.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.PutAsync(requestUri, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                //请求状态不对的时候抛出异常信息
                throw new HttpRequestException($"StatusCode：{(int)response.StatusCode}，ReasonPhrase：{response.ReasonPhrase}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(this Dictionary<string, string> paramters, string url, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            var httpContent = new FormUrlEncodedContent(paramters ?? new Dictionary<string, string>());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    httpContent.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.PostAsync(url, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                //请求状态不对的时候抛出异常信息
                throw new HttpRequestException($"StatusCode：{(int)response.StatusCode}，ReasonPhrase：{response.ReasonPhrase}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 转换成Get请求的Url接口地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static string ToGetUrl(this string url, Dictionary<string, string> paramters)
        {
            var text = paramters.ToGetParamters();
            return $"{url.TrimEnd('?', '&', '=')}?{text}";
        }

        /// <summary>
        /// 转换成Get请求参数
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static string ToGetParamters(this Dictionary<string, string> paramters)
        {
            string text = "";
            if (paramters == null || !paramters.Any())
            {
                return text;
            }

            foreach (KeyValuePair<string, string> paramter in paramters)
            {
                text += paramter.Key + "=" + paramter.Value + "&";
            }
            return text.TrimEnd('&');
        }

        /// <summary>
        /// 获取统一资源标识符
        /// </summary>
        /// <param name="conf">服务端配置</param>
        /// <returns></returns>
        public static Uri GetUri(this ServiceSettings conf)
        {
            if (conf == null)
            {
                throw new ArgumentNullException("ServiceSettings");
            }
            if (!string.IsNullOrWhiteSpace(conf?.IP))
            {
                return new Uri($"{conf.Scheme}://{conf?.IP}:{conf.Port}");
            }

            var currentIp = GetCurrentIP();
            return new Uri($"{conf.Scheme}://{currentIp}:{conf.Port}");
        }

        /// <summary>
        /// 获取当前服务器的Ip地址
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentIP()
        {
            try
            {
                var ipAddress = "";
                var addressInfo = NetworkInterface.GetAllNetworkInterfaces()
                    .Select(e => e.GetIPProperties())
                    .SelectMany(e => e.UnicastAddresses)
                    .Where(e => e.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(e.Address))
                    .FirstOrDefault();

                if (addressInfo != null && addressInfo.Address != null)
                {
                    //绑定Ip地址
                    ipAddress = $"{addressInfo.Address}";
                }

                if (string.IsNullOrEmpty(ipAddress))
                {
                    var hoostAddresses = Dns.GetHostAddresses(Dns.GetHostName())
                    .Where(e => e.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(e))
                    .FirstOrDefault();

                    if (hoostAddresses != null)
                    {
                        //绑定Ip地址
                        ipAddress = $"{hoostAddresses}";
                    }
                }
                return ipAddress;
            }
            catch
            {

            }
            return "127.0.0.1";
        }
    }
}