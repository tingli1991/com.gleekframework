using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 模型验证扩展方法
    /// </summary>
    public static class ModelValidExtensions
    {
        /// <summary>
        /// 错误信息枚举
        /// </summary>
        public static Type Type { get; set; }

        /// <summary>
        /// 注入模型验证
        /// </summary>
        /// <typeparam name="TFilterType"></typeparam>
        /// <typeparam name="EnumType"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IFilterMetadata Add<TFilterType, EnumType>(this FilterCollection source) where TFilterType : IFilterMetadata where EnumType : Enum
        {
            Type = typeof(EnumType);
            return source.Add<TFilterType>();
        }
    }
}