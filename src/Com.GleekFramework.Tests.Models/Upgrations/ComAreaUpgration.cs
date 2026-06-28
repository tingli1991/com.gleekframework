using Com.GleekFramework.MigrationSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 地区编码升级脚本
    /// </summary>
    [Upgration(1712500460000)]
    public class ComAreaUpgration : Upgration
    {
        /// <summary>
        /// 执行sql文件
        /// </summary>
        /// <returns></returns>
        public async override Task<IEnumerable<string>> ExecuteSqlFilesAsync()
        {
            return await Task.FromResult(new List<string>() { "com_area.sql" });
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <returns></returns>
        public override Task<IEnumerable<string>> ExecuteScriptsAsync()
        {
            return base.ExecuteScriptsAsync();
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        /// <returns></returns>
        public override Task ExecuteAsync()
        {
            return base.ExecuteAsync();
        }
    }
}