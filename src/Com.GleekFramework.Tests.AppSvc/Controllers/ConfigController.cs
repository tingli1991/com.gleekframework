using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 配置测试控制器
    /// </summary>
    [Route("config")]
    public class ConfigController : BaseController
    {
        /// <summary>
        /// 注入配置信息
        /// </summary>
        [Config(Models.ConfigConstant.SummariesKey)]
        public static string[] Summaries { get; set; }

        /// <summary>
        /// 配置选项
        /// </summary>
        [Config(Models.ConfigConstant.ConfigOptionsKey)]
        public static ConfigOptions ConfigOptions { get; set; }

        /// <summary>
        /// 服务依赖注入
        /// </summary>
        public DependencyService DependencyService { get; set; }

        /// <summary>
        /// 添加天气预报
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContractResult> AddAsync(WeatherForecastParam param)
        {
            return await Task.FromResult(new ContractResult().SetSuceccful());
        }

        /// <summary>
        /// 获取天气预报
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpGet("{app_id}/{status}")]
        public async Task<IEnumerable<WeatherForecastModel>> GetAsync([RouteRequired("app_id", "应用Id")] string appId, [RouteRequired("status", "状态")] bool status)
        {
            if (Summaries == null || Summaries.Length == 0)
            {
                return Enumerable.Empty<WeatherForecastModel>();
            }

            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }));
        }
    }
}