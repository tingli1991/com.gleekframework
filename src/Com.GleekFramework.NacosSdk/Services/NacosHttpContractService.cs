using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Http协议客户端
    /// </summary>
    public partial class NacosHttpContractService : NacosService, IBaseAutofac
    {
        /// <summary>
        /// Http客户端工厂类
        /// </summary>
        private static readonly HttpContractService HttpClientService = AutofacProvider.GetService<HttpContractService>();

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task<ContractResult> GetAsync(string serviceName, string action, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
             => await ExecuteAsync(serviceName, action, (url) => HttpClientService.GetAsync<ContractResult>(url, param, headers), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task<ContractResult<T>> GetAsync<T>(string serviceName, string action, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.GetAsync<T>(url, param, headers), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发起Delete请求
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task<ContractResult> DeleteAsync(string serviceName, string action, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.DeleteAsync<ContractResult>(url, param, headers), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发起Delete请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task<ContractResult<T>> DeleteAsync<T>(string serviceName, string action, Dictionary<string, string> param = null, Dictionary<string, string> headers = null)
             => await ExecuteAsync(serviceName, action, (url) => HttpClientService.DeleteAsync<T>(url, param, headers), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult> PostAsync<R>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PostAsync<R, ContractResult>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">响应模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult<T>> PostAsync<R, T>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PostAsync<R, T>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Put请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult> PutAsync<R>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PutAsync<R, ContractResult>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Put请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">响应模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult<T>> PutAsync<R, T>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PutAsync<R, T>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Patch请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult> PatchAsync<R>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PatchAsync<R, ContractResult>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);

        /// <summary>
        /// 发送Patch请求
        /// </summary>
        /// <typeparam name="R">请求模型</typeparam>
        /// <typeparam name="T">响应模型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="action">接口路径</param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ContractResult<T>> PatchAsync<R, T>(string serviceName, string action, R data, Dictionary<string, string> headers = null, Dictionary<string, string> param = null)
            => await ExecuteAsync(serviceName, action, (url) => HttpClientService.PatchAsync<R, T>(url, data, headers, param), NacosConstant.DEFAULT_HTTP_SCHEME);
    }
}