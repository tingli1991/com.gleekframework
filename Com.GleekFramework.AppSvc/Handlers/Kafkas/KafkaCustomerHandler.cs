using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NLogSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义Kafka消费处理类
    /// </summary>
    public class KafkaCustomerHandler : IKafkaHandler
    {
        /// <summary>
        /// 日志服务
        /// </summary>
        public NLogService NLogService { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public Enum ActionKey => MessageType.CUSTOMER_TEST_KAFKA_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var beginTime = DateTime.Now.ToCstTime();
            var param = messageBody.GetData<StudentParam>();//请求参数
            var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMicroseconds;//耗时时间
            var response = new ContractResult().SetSuceccful(messageBody.SerialNo);//相应结果
            NLogService.Info($"请求参数：{param.JsonCompressAndEscape()}", messageBody.SerialNo, messageBody.Headers.GetUrl(), totalMilliseconds);
            return await Task.FromResult(response);
        }
    }
}