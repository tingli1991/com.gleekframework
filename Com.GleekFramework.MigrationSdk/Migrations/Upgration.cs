using FluentMigrator.Builders.Execute;
using FluentMigrator.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本升级程序基类
    /// </summary>
    public abstract class Upgration : IUpgration
    {
        /// <summary>
        /// 版本变更上下文
        /// </summary>
        public IMigrationContext Context { get; set; }

        /// <summary>
        /// 数据库脚本执行
        /// </summary>
        public IExecuteExpressionRoot Execute { get; set; }

        /// <summary>
        /// 升级过程中需要执行的代码逻辑
        /// </summary>
        /// <returns></returns>
        public virtual Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 升级过程中需要执行的脚本
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<string>> ExecuteScriptsAsync()
        {
            return Task.FromResult<IEnumerable<string>>(new List<string>());
        }

        /// <summary>
        /// 升级过程中需要执行的脚本
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<string>> ExecuteSqlFilesAsync()
        {
            return Task.FromResult<IEnumerable<string>>(new List<string>());
        }
    }
}