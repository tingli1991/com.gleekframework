using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 自定义授权特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public abstract class CustomAuthorizeAttribute : CoustomAttribute
    {
        /// <summary>
        /// 异步调用授权方法
        /// </summary>
        /// <param name="context">消息内容</param>
        /// <returns></returns>
        public virtual async Task<ContractResult> OnAuthorizationAsync(CustomAuthorizationContext context)
        {
            ContractResult response = default;
            return await Task.FromResult(response);
        }
    }
}