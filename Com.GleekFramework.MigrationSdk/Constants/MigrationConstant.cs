using System.Collections.Generic;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本变更常量配置
    /// </summary>
    public static partial class MigrationConstant
    {
        /// <summary>
        /// 主键字段名称
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// 基础列
        /// </summary>
        public static readonly List<string> BaseColumns = new List<string>() { "Version", "IsDeleted", "UpdateTime", "CreateTime", "Extend", "Remark" };
    }
}