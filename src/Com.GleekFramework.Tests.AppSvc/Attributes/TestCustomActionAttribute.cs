using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;

namespace Com.GleekFramework.AppSvc.Attributes
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestCustomActionAttribute : CustomActionAttribute
    {
        /// <summary>
        /// 排序(升序排序)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 方法执行之前调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnActionExecutingAsync(CustomActionExecutingContext context)
        {
            return base.OnActionExecutingAsync(context);
        }

        /// <summary>
        /// 方法执行完成之后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnActionExecutedAsync(CustomActionExecutedContext context)
        {
            return base.OnActionExecutedAsync(context);
        }
    }
}