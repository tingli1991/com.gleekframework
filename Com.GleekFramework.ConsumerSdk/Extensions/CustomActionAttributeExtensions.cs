using Com.GleekFramework.ContractSdk;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 自定义行为扩展类
    /// </summary>
    public static partial class CustomActionAttributeExtensions
    {
        /// <summary>
        /// 触发方法调用之前的执行结果
        /// </summary>
        /// <param name="coustomAttributeList">自定义特性列表</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public static async Task<ContractResult> OnActionExecutingAsync(this IEnumerable<CustomActionAttribute> coustomAttributeList, CustomActionExecutingContext context)
        {
            var isSuccess = true;//是否处理成功
            ContractResult result = default;//返回结果
            if (coustomAttributeList != null && coustomAttributeList.Any())
            {
                foreach (var coustomAttribute in coustomAttributeList.OrderBy(e => e.Order))
                {
                    result = await coustomAttribute.OnActionExecutingAsync(context);
                    if (result != null && !result.IsSuceccful())
                    {
                        break;
                    }
                }
            }
            return !isSuccess ? result : null;
        }

        /// <summary>
        /// 触发方法调用之前的结果
        /// </summary>
        /// <param name="coustomAttributeList"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<ContractResult> OnActionExecutedAsync(this IEnumerable<CustomActionAttribute> coustomAttributeList, CustomActionExecutedContext context)
        {
            ContractResult result = default;//返回结果
            if (context?.Result != null && !context.Result.IsSuceccful())
            {
                //上层管道异常直接返回
                return context.Result;
            }
            else
            {
                var isSuccess = true;//是否处理成功
                if (coustomAttributeList != null && coustomAttributeList.Any())
                {
                    foreach (var coustomAttribute in coustomAttributeList.OrderBy(e => e.Order))
                    {
                        result = await coustomAttribute.OnActionExecutedAsync(context);
                        if (result != null && !result.IsSuceccful())
                        {
                            break;
                        }
                    }
                }
                return !isSuccess ? result : null;
            }
        }
    }
}