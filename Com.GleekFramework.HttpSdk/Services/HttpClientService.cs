using Com.GleekFramework.AutofacSdk;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// Http客户端服务
    /// </summary>
    public partial class HttpClientService : IBaseAutofac
    {
        /// <summary>
        /// Http客户端工厂类
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public Task<string> GetAsync(string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            return HttpClientFactory.GetAsync<string>(HttpContextAccessor, url, param, headers);
        }

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            return HttpClientFactory.GetAsync<T>(HttpContextAccessor, url, param, headers);
        }

        /// <summary>
        /// 发起Delete请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public Task<string> DeleteAsync(string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            return HttpClientFactory.DeleteAsync<string>(HttpContextAccessor, url, param, headers);
        }

        /// <summary>
        /// 发起Delete请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">接口地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public Task<T> DeleteAsync<T>(string url, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
        {
            return HttpClientFactory.DeleteAsync<T>(HttpContextAccessor, url, param, headers);
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<string> PostAsync<R>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PostAsync<R, string>(HttpContextAccessor, url, data, headers, param);
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<T> PostAsync<R, T>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PostAsync<R, T>(HttpContextAccessor, url, data, headers, param);
        }

        /// <summary>
        /// 发送Put请求
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<string> PutAsync<R>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PutAsync<R, string>(HttpContextAccessor, url, data, headers);
        }

        /// <summary>
        /// 发送Put请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<T> PutAsync<R, T>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PutAsync<R, T>(HttpContextAccessor, url, data, headers);
        }

        /// <summary>
        /// 发送Patch请求
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<string> PatchAsync<R>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PatchAsync<R, string>(HttpContextAccessor, url, data, headers, param);
        }

        /// <summary>
        /// 发送Patch请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public Task<T> PatchAsync<R, T>(string url, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
        {
            return HttpClientFactory.PatchAsync<R, T>(HttpContextAccessor, url, data, headers, param);
        }
    }
}