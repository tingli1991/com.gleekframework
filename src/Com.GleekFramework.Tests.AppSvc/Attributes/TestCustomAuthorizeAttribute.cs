using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;

namespace Com.GleekFramework.AppSvc.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class TestCustomAuthorizeAttribute : CustomAuthorizeAttribute
    {
        /// <summary>
        /// 排序(升序排序)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 方法调用之前，比 CustomActionAttribute 更早
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnAuthorizationAsync(CustomAuthorizationContext context)
        {
            return base.OnAuthorizationAsync(context);
        }
    }
}