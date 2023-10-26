using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Kafka���Կ�����
    /// </summary>
    [Route("kafka")]
    public class KafkaController : BaseController
    {
        /// <summary>
        /// ��ˮ��������
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Http�ͻ��˹�����
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Http����������
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// Kafka�ͻ��˷���
        /// </summary>
        public KafkaClientService KafkaClientService { get; set; }

        /// <summary>
        /// �ӿڲ���
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
                    Name = $"����_{i}"
                };

                var headers = new Dictionary<string, string>()
                {
                    { "test-header-key", "��ȷ��ͷ����ֵ��" },
                    { "test_header_key", "�����Ϲ����ͷ����ֵ��" }
                };

                var dataList = new List<StudentParam>() { param };
                var response = KafkaClientService.PublishManyAsync(RabbitConfig.KafkaDefaultClientHosts, KafkaTopicConstant.DEFAULT_TOPIC, MessageType.CUSTOMER_TEST_KAFKA_NAME, dataList, serialNo, headers);
                Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ������ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}