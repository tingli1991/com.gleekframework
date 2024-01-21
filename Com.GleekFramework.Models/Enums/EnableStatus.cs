using System.ComponentModel;

namespace Com.GleekFramework.Models.Enums
{
    /// <summary>
    /// 启用状态
    /// </summary>
    public enum EnableStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 10,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 20
    }
}