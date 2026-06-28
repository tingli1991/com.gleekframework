using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 表备注特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class CommentAttribute : Attribute
    {
        /// <summary>
        /// 备注字段
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="comment"></param>
        public CommentAttribute(string comment)
        {
            Comment = comment;
        }
    }
}