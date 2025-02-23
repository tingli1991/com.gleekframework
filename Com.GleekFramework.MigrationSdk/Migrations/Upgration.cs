using System;
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
        /// 服务实现类
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 数据库实现类
        /// </summary>
        public IDatabaseProvider DatabaseProvider { get; set; }

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