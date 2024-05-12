using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 发布订阅模式消费 
    /// </summary>
    public class RabbitSubscribeTestHandler : RabbitSubscribeHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// 交换机名称
        /// </summary>
        public override string ExchangeName => RabbitQueueConstant.WorkCustomerExchangeName;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}