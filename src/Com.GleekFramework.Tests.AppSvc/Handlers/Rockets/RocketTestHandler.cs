using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 测试消费
    /// </summary>
    public class RocketTestHandler : IRocketHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public Enum ActionKey => MessageType.CUSTOMER_TEST_KAFKA_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}