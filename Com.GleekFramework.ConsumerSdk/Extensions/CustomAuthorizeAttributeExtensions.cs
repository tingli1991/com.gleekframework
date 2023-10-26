using Com.GleekFramework.ContractSdk;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 授权特性扩展
    /// </summary>
    public static partial class CustomAuthorizeAttributeExtensions
    {
        /// <summary>
        /// 异步调用授权方法
        /// </summary>
        /// <param name="coustomAttributeList"></param>
        /// <param name="context">消息内容</param>
        /// <returns></returns>
        public static async Task<ContractResult> OnAuthorizationAsync(this IEnumerable<CustomAuthorizeAttribute> coustomAttributeList, CustomAuthorizationContext context)
        {
            ContractResult result = default;//返回结果
            var isSuccess = true;//是否处理成功
            if (coustomAttributeList != null && coustomAttributeList.Any())
            {
                foreach (var coustomAttribute in coustomAttributeList.OrderBy(e => e.Order))
                {
                    var response = await coustomAttribute.OnAuthorizationAsync(context);
                    if (response != null && !response.IsSuceccful())
                    {
                        isSuccess = false;
                        result = response;
                        break;
                    }
                }
            }
            return !isSuccess ? result : null;
        }
    }
}