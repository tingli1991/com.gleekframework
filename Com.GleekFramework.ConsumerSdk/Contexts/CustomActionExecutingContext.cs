using Com.GleekFramework.ContractSdk;
using System;
using System.Collections.Generic;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 自定义行为上下文
    /// </summary>
    public class CustomActionExecutingContext
    {
        /// <summary>
        /// 处理方法的实例对象
        /// </summary>
        public virtual object Handler { get; set; }

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
        /// <param name="context">授权的上下文</param>
        /// <returns></returns>
        public static CustomActionExecutingContext CreateActionExecutingContext(CustomAuthorizationContext context)
        {
            if (context == null)
            {
                throw new NullReferenceException(nameof(context));
            }

            return new CustomActionExecutingContext()
            {
                Handler = context?.Handler,
                MessageBody = context?.MessageBody,
                ActionArguments = context.ActionArguments ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 创建自定义行为上下对象
        /// </summary>
        /// <param name="handler">处理方法的实例对象</param>
        /// <param name="messageBody">消息内容</param>
        /// <param name="actionArguments">行为参数字典</param>
        /// <returns></returns>
        public static CustomActionExecutingContext CreateActionExecutingContext(object handler, MessageBody messageBody, Dictionary<string, object> actionArguments = null)
        {
            return new CustomActionExecutingContext()
            {
                Handler = handler,
                MessageBody = messageBody,
                ActionArguments = actionArguments ?? new Dictionary<string, object>()
            };
        }
    }
}