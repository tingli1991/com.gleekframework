using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 索引特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class IndexAttribute : Attribute
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 索引是否唯一
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// 是否是升序
        /// </summary>
        public bool IsAsc { get; set; } = true;

        /// <summary>
        /// 属性集合
        /// </summary>
        public IReadOnlyList<string> PropertyNames { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyNames">属性名称集合</param>
        public IndexAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames.ToList();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="propertyNames">属性名称集合</param>
        public IndexAttribute(string name, params string[] propertyNames)
        {
            Name = name;
            PropertyNames = propertyNames.ToList();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="isUnique">是否唯一索引</param>
        /// <param name="propertyNames">属性名称集合</param>
        public IndexAttribute(string name, bool isUnique, params string[] propertyNames)
        {
            Name = name;
            IsUnique = isUnique;
            PropertyNames = propertyNames.ToList();
        }
    }
}