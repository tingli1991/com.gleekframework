using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 自定义行为特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public abstract class CustomActionAttribute : CoustomAttribute
    {
        /// <summary>
        /// 方法执行之前
        /// </summary>
        /// <param name="context">上下文</param>
        public virtual async Task<ContractResult> OnActionExecutingAsync(CustomActionExecutingContext context)
        {
            ContractResult response = default;
            return await Task.FromResult(response);
        }

        /// <summary>
        /// 方法执行之后运行
        /// </summary>
        /// <param name="context">消息内容</param>
        public virtual async Task<ContractResult> OnActionExecutedAsync(CustomActionExecutedContext context)
        {
            ContractResult response = default;
            return await Task.FromResult(response);
        }
    }
}