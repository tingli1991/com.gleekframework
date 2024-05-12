using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 表示应将属性或类从映射中排除
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotMapAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotMapAttribute()
        {

        }
    }
}