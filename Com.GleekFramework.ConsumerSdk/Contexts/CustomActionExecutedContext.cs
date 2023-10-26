using Com.GleekFramework.ContractSdk;
using System.Collections.Generic;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 方法调用之后的上下文
    /// </summary>
    public class CustomActionExecutedContext
    {
        /// <summary>
        /// 处理方法的实例对象
        /// </summary>
        public virtual object Handler { get; set; }

        /// <summary>
        /// 调用之前的返回结果
        /// </summary>
        public virtual ContractResult Result { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual MessageBody MessageBody { get; set; }

        /// <summary>
        /// 行为参数字典
        /// </summary>
        public virtual Dictionary<string, object> ActionArguments { get; set; }

        /// <summary>
        /// 创建自定义行为上下对象
        /// </summary>
        /// <param name="actionExecutingContext">处理方法的实例对象</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CustomActionExecutedContext CreateCustomActionExecutedContext(CustomActionExecutingContext actionExecutingContext, ContractResult result)
        {
            return CreateCustomActionExecutedContext(actionExecutingContext?.Handler, actionExecutingContext?.MessageBody, result, actionExecutingContext?.ActionArguments);
        }

        /// <summary>
        /// 创建自定义行为上下对象
        /// </summary>
        /// <param name="handler">处理方法的实例对象</param>
        /// <param name="messageBody">消息内容</param>
        /// <param name="result"></param>
        /// <param name="actionArguments">行为参数字典</param>
        /// <returns></returns>
        public static CustomActionExecutedContext CreateCustomActionExecutedContext(object handler, MessageBody messageBody, ContractResult result, Dictionary<string, object> actionArguments = null)
        {
            return new CustomActionExecutedContext()
            {
                Result = result,
                Handler = handler,
                MessageBody = messageBody,
                ActionArguments = actionArguments ?? new Dictionary<string, object>()
            };
        }
    }
}