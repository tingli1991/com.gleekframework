using System;
using System.ComponentModel;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 版本对比方式
    /// </summary>
    [Serializable]
    public enum VersionCompare
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        EQ = 10,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NQ = 20,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        LT = 30,

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        GT = 40,

        /// <summary>
        /// 小于或等于
        /// </summary>
        [Description("小于或等于")]
        LTE = 50,

        /// <summary>
        /// 大于或等于
        /// </summary>
        [Description("大于或等于")]
        GTE = 60
    }
}