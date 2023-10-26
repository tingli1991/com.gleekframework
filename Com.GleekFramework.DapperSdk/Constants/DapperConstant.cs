namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Dapper常量
    /// </summary>
    public static partial class DapperConstant
    {
        /// <summary>
        /// 数据库访问超市时间的秒数
        /// </summary>
        public const int DEFAULT_TIMEOUT_SECONDS = 30;

        /// <summary>
        /// MSSQL默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_MSSQL_CONNECTION_NAME = "ConnectionStrings:MsSQLConnectionHost";

        /// <summary>
        /// MYSQL默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_MYSQL_CONNECTION_NAME = "ConnectionStrings:MySQLConnectionHost";

        /// <summary>
        /// PGSQL默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_PGSQL_CONNECTION_NAME = "ConnectionStrings:PgSQLConnectionHost";

        /// <summary>
        /// SQLLITE默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_SQLLITE_CONNECTION_NAME = "ConnectionStrings:SQLiteConnectionHost";

        /// <summary>
        /// ORACLEE默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_ORACLE_CONNECTION_NAME = "ConnectionStrings:OracleConnectionHost";
    }
}