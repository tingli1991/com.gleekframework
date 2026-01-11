using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义栈消费处理类
    /// </summary>
    public class StackCustomerHandler : StackHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_STACK_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var currentTime = DateTime.Now.ToCstTime();
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (currentTime - beginTime).TotalMilliseconds;
            Console.WriteLine($"时间戳：{messageBody.TimeStamp}，当前时间：{currentTime}，开始时间：{beginTime}，主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}");
            //Console.WriteLine($"时间戳：{messageBody.TimeStamp}，当前时间：{currentTime}，开始时间：{beginTime}，主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}