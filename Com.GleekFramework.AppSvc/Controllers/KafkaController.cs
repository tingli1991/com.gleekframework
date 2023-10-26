using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Kafka测试控制器
    /// </summary>
    [Route("kafka")]
    public class KafkaController : BaseController
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
        /// Kafka客户端服务
        /// </summary>
        public KafkaClientService KafkaClientService { get; set; }

        /// <summary>
        /// 接口测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam()
                {
                    Id = i,
                    Name = $"张三_{i}"
                };

                var headers = new Dictionary<string, string>()
                {
                    { "test-header-key", "正确的头部键值对" },
                    { "test_header_key", "不符合规则的头部键值对" }
                };

                var dataList = new List<StudentParam>() { param };
                var response = KafkaClientService.PublishManyAsync(RabbitConfig.KafkaDefaultClientHosts, KafkaTopicConstant.DEFAULT_TOPIC, MessageType.CUSTOMER_TEST_KAFKA_NAME, dataList, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}