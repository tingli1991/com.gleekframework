using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos基础服务
    /// </summary>
    public class NacosService : IBaseAutofac
    {
        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="action"></param>
        /// <param name="func"></param>
        /// <param name="scheme">契约</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(string serviceName, string action, Func<string, Task<T>> func, string scheme = "")
            => await func(await NacosServiceProvider.GetHostUrlAsync(serviceName, action, scheme));

        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="action"></param>
        /// <param name="func"></param>
        /// <param name="scheme">契约</param>
        /// <returns></returns>
        public async Task<ContractResult<T>> ExecuteAsync<T>(string serviceName, string action, Func<string, Task<ContractResult<T>>> func, string scheme = "")
            => await func(await NacosServiceProvider.GetHostUrlAsync(serviceName, action, scheme));
    }
}