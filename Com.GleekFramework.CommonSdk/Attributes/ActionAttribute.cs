using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 行为特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ActionAttribute : Attribute
    {
        /// <summary>
        /// 行为名称键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键</param>
        public ActionAttribute(string key)
        {
            Key = key;
        }
    }
}