using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 允许用户授权范围拓展类
    /// </summary>
    public static class AllowAuthorizeExtensions
    {
        /// <summary>
        /// 获取允许用户授权范围特性
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AllowAuthorizeAttribute GetAllowAuthorizeAttribute(this AuthorizationFilterContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;//获取当前Action和Controller的描述信息
            var methodAllowAuthorizeAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<AllowAuthorizeAttribute>();//方法上的匿名函数特性
            var controllerAllowAuthorizeAttribute = actionDescriptor.ControllerTypeInfo.GetCustomAttribute<AllowAuthorizeAttribute>();//控制器上的匿名函数特性
            var allowAuthorizeAttribute = methodAllowAuthorizeAttribute ?? controllerAllowAuthorizeAttribute ?? new AllowAuthorizeAttribute();
            allowAuthorizeAttribute.VerifyStatus = allowAuthorizeAttribute.VerifyToken && allowAuthorizeAttribute.VerifyStatus;//修正状态验证的逻辑
            return allowAuthorizeAttribute;
        }
    }
}