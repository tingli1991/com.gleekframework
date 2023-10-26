using Com.GleekFramework.AutofacSdk;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Oracle数据仓储
    /// </summary>
    public partial class OracleRepository : DapperRepository, IBaseAutofac
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_ORACLE_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection GetConnection()
        {
            return new OracleConnection(ConnectionString);
        }
    }
}