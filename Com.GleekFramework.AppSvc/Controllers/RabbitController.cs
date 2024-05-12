using Com.GleekFramework.AttributeSdk;
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
        /// RPC客户端服务
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

        /// <summary>
        /// 工作模式可客户端
        /// </summary>
        public RabbitWorkClientService RabbitWorkClientService { get; set; }

        /// <summary>
        /// 发布订阅模式客户端
        /// </summary>
        public RabbitSubscribeClientService RabbitSubscribeClientService { get; set; }

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
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Work测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("work")]
        public async Task WorkAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitWorkClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerQueue, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Subscribe测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("subscribe")]
        public async Task SubscribeAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitSubscribeClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerExchangeName, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}