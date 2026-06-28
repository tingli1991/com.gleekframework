using Microsoft.AspNetCore.Mvc;
using System;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// Autofac基础常量
    /// </summary>
    public static class AutofacConstant
    {
        /// <summary>
        /// 用于实现属性注入的基础接口类型
        /// </summary>
        public static readonly Type BASEAUTOFAC_TYPE = typeof(IBaseAutofac);

        /// <summary>
        /// 用于实现属性注入的基础接口类型
        /// </summary>
        public static readonly Type BASEAUTOFAC_GENERIC_TYPE = typeof(IBaseAutofac<>);

        /// <summary>
        /// 基础控制器属性注入的基础接口类型
        /// </summary>
        public static readonly Type CONTROLLERBASE_TYPE = typeof(ControllerBase);
    }
}