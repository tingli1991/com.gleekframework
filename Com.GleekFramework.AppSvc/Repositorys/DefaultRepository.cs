using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.Models;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 默认测试仓储(读写)
    /// </summary>
    public class DefaultRepository<T> : MySqlRepository<T> where T : ITable
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DatabaseConstant.DefaultMySQLHostsKey;
    }
}
