using Com.GleekFramework.AppSvc.Attributes;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义队列消费处理类
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [TestCustomAction, TestCustomAuthorize]
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}