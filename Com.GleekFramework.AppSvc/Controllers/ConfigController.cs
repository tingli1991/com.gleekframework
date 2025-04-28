using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        /// 测试配置
        /// </summary>
        [Config(Models.ConfigConstant.ConfigOptionsKey)]
        public static ConfigOptions ConfitOptions { get; set; }

        /// <summary>
        /// 测试依赖注入
        /// </summary>
        public DependencyService DependencyService { get; set; }

        /// <summary>
        /// 新增方法
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
        /// <param name="id">id(用于测试字段必填)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<WeatherForecastModel>> GetAsync([System.ComponentModel.DataAnnotations.Required] int id)
        {
            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }));
        }
    }
}