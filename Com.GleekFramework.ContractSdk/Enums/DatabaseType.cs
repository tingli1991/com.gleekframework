using System;
using System.ComponentModel;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    [Serializable]
    public enum DatabaseType
    {
        /// <summary>
        /// MsSQL数据库
        /// </summary>
        [Description("MsSQL数据库")]
        MsSQL = 10,

        /// <summary>
        /// MySQL数据库
        /// </summary>
        [Description("MySQL数据库")]
        MySQL = 20,

        /// <summary>
        /// PgSQL数据库
        /// </summary>
        [Description("PgSQL数据库")]
        PgSQL = 30,

        /// <summary>
        /// SQLite数据库
        /// </summary>
        [Description("SQLite数据库")]
        SQLite = 40,
    }
}