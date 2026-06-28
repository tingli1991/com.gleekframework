using Com.GleekFramework.ContractSdk;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消费者执行拓展类
    /// </summary>
    public static partial class CoustomExecuteExtensions
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="handler">对象实例</param>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static async Task<ContractResult> ExecuteAsync(this IHandler handler, MessageBody messageBody)
        {
            return await CoustomExecuteProvider.ExecuteAsync(handler, messageBody);
        }
    }
}