using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 队列测试方法定义类
    /// </summary>
    public class RabbitRpcTestHandler : RabbitRpcHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            //Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}