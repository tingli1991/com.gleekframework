namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本变更配置选项
    /// </summary>
    public class MigrationOptions
    {
        /// <summary>
        /// 版本迁移开关
        /// </summary>
        public bool MigrationSwitch { get; set; } = true;

        /// <summary>
        /// 版本升级开关
        /// </summary>
        public bool UpgrationSwitch { get; set; } = true;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 注入的数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }
    }
}