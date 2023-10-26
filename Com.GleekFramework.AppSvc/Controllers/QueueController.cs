using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// ���ض��в��Կ�����
    /// </summary>
    [Route("queue")]
    public class QueueController : BaseController
    {
        /// <summary>
        /// ��ˮ��������
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// �ͻ��˷���
        /// </summary>
        public QueueClientService QueueClientService { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                //var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"����_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "�����Ϲ����ͷ����ֵ��" }, { "test-header-key", "��ȷ��ͷ����ֵ��" } };
                await QueueClientService.PublishAsync(MessageType.CUSTOMER_TEST_QUEUE_NAME, param, serialNo, headers);
                //Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ������ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}