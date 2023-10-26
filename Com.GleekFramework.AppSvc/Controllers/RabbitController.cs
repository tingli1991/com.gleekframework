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
        /// Http�ͻ��˹�����
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Http����������
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// RPC�ͻ��˷���
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

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
                //var response1 = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, "com.gleekframework.customer.queue.rpc", MessageType.CUSTOMER_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"��Ϣ��ʼʱ�䣺{beginTime:yyyy-MM-dd HH:mm:ss}����Ϣ������ʱ��{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}