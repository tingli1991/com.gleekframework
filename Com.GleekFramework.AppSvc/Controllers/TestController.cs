using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public RocketClientService RocketClientService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var topic = "test_topic_01";
            var host = "";
            var deliverTimeMillis = 10;
            var weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "测试", TemperatureC = 100 };
            await RocketClientService.PublishMessageBodyAsync(host, topic, MessageType.TEST_QUERY_RPC_NAME, weatherForecastInfo, deliverTimeMillis);

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}