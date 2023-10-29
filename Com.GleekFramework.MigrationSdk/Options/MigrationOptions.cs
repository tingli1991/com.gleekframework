using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本变更配置选项
    /// </summary>
    public class MigrationOptions
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 需要扫描的程序集
        /// </summary>
        public IEnumerable<Assembly> Assemblys { get; set; }
    }
}