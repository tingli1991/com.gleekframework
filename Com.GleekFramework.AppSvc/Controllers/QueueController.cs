using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 本地队列测试控制器
    /// </summary>
    [Route("queue")]
    public class QueueController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 客户端服务
        /// </summary>
        public QueueClientService QueueClientService { get; set; }

        /// <summary>
        /// 生产测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                await QueueClientService.PublishAsync(MessageType.CUSTOMER_TEST_QUEUE_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}