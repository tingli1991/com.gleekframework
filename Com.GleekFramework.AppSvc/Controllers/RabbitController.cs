using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// RabbitMQ测试控制器
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Http客户端工厂类
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// RPC客户端服务
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

        /// <summary>
        /// RPC测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("rpc")]
        public async Task RpcAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                //var response1 = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, "com.geekframework.customer.queue.rpc", MessageType.CUSTOMER_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}