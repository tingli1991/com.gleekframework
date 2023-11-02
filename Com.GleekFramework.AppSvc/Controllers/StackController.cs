using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// ����ջ���Կ�����
    /// </summary>
    [Route("stack")]
    public class StackController : BaseController
    {
        /// <summary>
        /// ��ˮ��������
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// �ͻ��˷���
        /// </summary>
        public StackClientService StackClientService { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"����_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "�����Ϲ����ͷ����ֵ��" }, { "test-header-key", "��ȷ��ͷ����ֵ��" } };
                await StackClientService.PublishAsync(MessageType.CUSTOMER_TEST_STACK_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}