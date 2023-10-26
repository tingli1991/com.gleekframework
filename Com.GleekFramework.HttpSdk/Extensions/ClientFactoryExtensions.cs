using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// Http客户端工厂拓展类
    /// </summary>
    public static partial class ClientFactoryExtensions
    {
        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory">工厂类</param>
        /// <param name="context">上下文</param>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IHttpClientFactory factory, IHttpContextAccessor context, string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return default;
                }

                var requestUri = url.ToGetUrl(param);
                using HttpClient client = factory.CreateClient(context, headers);
                using var response = await client.GetAsync(requestUri);
                return await response.ReadAsStringAsync<T>();//输出响应结果
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发起Delete请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory">工厂类</param>
        /// <param name="context">上下文</param>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public static async Task<T> DeleteAsync<T>(this IHttpClientFactory factory, IHttpContextAccessor context, string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return default;
                }

                var requestUri = url.ToGetUrl(param);
                using HttpClient client = factory.CreateClient(context, headers);
                using var response = await client.DeleteAsync(requestUri);
                return await response.ReadAsStringAsync<T>();//输出响应结果
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">返回模型</typeparam>
        /// <param name="factory">工厂类</param>
        /// <param name="context">上下文</param>
        /// <param name="url">接口地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">头部信息</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<T> PostAsync<R, T>(this IHttpClientFactory factory, IHttpContextAccessor context, string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return default;
                }

                var requestUri = url.ToGetUrl(param);
                var contentType = headers.GetContentType();
                var httpContent = data.ToStringContent();
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using var client = factory.CreateClient(context, headers);//创建客户端对象
                using var response = await client.PostAsync(requestUri, httpContent);
                return await response.ReadAsStringAsync<T>();//输出响应结果
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送Put请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">返回模型</typeparam>
        /// <param name="factory">工厂类</param>
        /// <param name="context">上下文</param>
        /// <param name="url">接口地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">头部信息</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<T> PutAsync<R, T>(this IHttpClientFactory factory, IHttpContextAccessor context, string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return default;
                }

                var requestUri = url.ToGetUrl(param);
                var contentType = headers.GetContentType();
                var httpContent = data.ToStringContent();
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using var client = factory.CreateClient(context, headers);//创建客户端对象
                using var response = await client.PutAsync(requestUri, httpContent);
                return await response.ReadAsStringAsync<T>();//输出响应结果
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送Patch请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">返回模型</typeparam>
        /// <param name="factory">工厂类</param>
        /// <param name="context">上下文</param>
        /// <param name="url">接口地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">头部信息</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<T> PatchAsync<R, T>(this IHttpClientFactory factory, IHttpContextAccessor context, string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return default;
                }

                var requestUri = url.ToGetUrl(param);
                var contentType = headers.GetContentType();
                var httpContent = data.ToStringContent();
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using var client = factory.CreateClient(context, headers);//创建客户端对象
                using var response = await client.PutAsync(requestUri, httpContent);
                return await response.ReadAsStringAsync<T>();//输出响应结果
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 读取响应内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<T> ReadAsStringAsync<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                //请求状态不对的时候抛出异常信息
                throw new HttpRequestException($"StatusCode：{(int)response.StatusCode}，ReasonPhrase：{response.ReasonPhrase}");
            }
            var responseJsonValue = await response.Content.ReadAsStringAsync();
            return responseJsonValue.DeserializeObject<T>();//反序列化请求参数
        }

        /// <summary>
        /// 创建客户端对象
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="context"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static HttpClient CreateClient(this IHttpClientFactory factory, IHttpContextAccessor context, Dictionary<string, string> headers)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Clear();//清楚请求头
            client.DefaultRequestHeaders.AddHeaders(headers);//追加自定义请求头
            client.DefaultRequestHeaders.AddHeaders(context.ToHeaders());//追加上下文请求头
            return client;
        }
    }
}