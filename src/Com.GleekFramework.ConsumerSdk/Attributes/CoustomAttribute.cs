using System;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 自定义特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public abstract class CoustomAttribute : Attribute
    {
        /// <summary>
        /// 顺序/排序
        /// </summary>
        public abstract int Order { get; }
    }
}