using System.ComponentModel;

namespace Com.GleekFramework.Models.Enums
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Man = 10,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        WoMan = 20,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 30
    }
}