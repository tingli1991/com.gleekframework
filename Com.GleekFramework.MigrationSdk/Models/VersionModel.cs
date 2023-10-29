using System;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public class VersionModel
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime AppliedOn { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}