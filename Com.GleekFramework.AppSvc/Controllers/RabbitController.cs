using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// RabbitMQ���Կ�����
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// ��ˮ��������
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// RPC�ͻ��˷���
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

        /// <summary>
        /// ����ģʽ�ɿͻ���
        /// </summary>
        public RabbitWorkClientService RabbitWorkClientService { get; set; }

        /// <summary>
        /// ��������ģʽ�ͻ���
        /// </summary>
        public RabbitSubscribeClientService RabbitSubscribeClientService { get; set; }

        /// <summary>
        /// RPC����
        /// </summary>
        /// <returns></returns>
        [HttpPost("rpc")]
        public async Task RpcAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"����_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "�����Ϲ����ͷ����ֵ��" }, { "test-header-key", "��ȷ��ͷ����ֵ��" } };
                var response = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ�����ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Work����
        /// </summary>
        /// <returns></returns>
        [HttpPost("work")]
        public async Task WorkAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"����_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "�����Ϲ����ͷ����ֵ��" }, { "test-header-key", "��ȷ��ͷ����ֵ��" } };
                var response = RabbitWorkClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerQueue, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ�����ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Subscribe����
        /// </summary>
        /// <returns></returns>
        [HttpPost("subscribe")]
        public async Task SubscribeAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"����_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "�����Ϲ����ͷ����ֵ��" }, { "test-header-key", "��ȷ��ͷ����ֵ��" } };
                var response = RabbitSubscribeClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerExchangeName, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ�����ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}