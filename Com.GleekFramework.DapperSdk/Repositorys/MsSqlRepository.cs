using Com.GleekFramework.AutofacSdk;
using System.Data;
using System.Data.SqlClient;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// MsSql仓储服务
    /// </summary>
    public partial class MsSqlRepository : DapperRepository, IBaseAutofac
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_MSSQL_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}