using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 消息类型拓展类
    /// </summary>
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static T GetData<T>(this MessageBody messageBody)
            where T : class
        {
            T result = default;
            if (messageBody is MessageBody<object> messageValue)
            {
                if (messageValue?.Data == null)
                {
                    return result;
                }

                var jsonValue = messageValue.Data.ToString();
                result = JsonConvert.DeserializeObject<T>(jsonValue);
            }
            else if (messageBody is MessageBody<T> messageValues)
            {
                result = messageValues.Data;
            }
            else
            {
                throw new InvalidOperationException(nameof(messageBody));
            }
            return result;
        }

        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static async Task<T> GetDataAsync<T>(this MessageBody messageBody)
            where T : class
        {
            var data = messageBody.GetData<T>();
            return await Task.FromResult(data);
        }
    }
}