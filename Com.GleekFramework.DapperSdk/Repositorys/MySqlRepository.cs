using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ContractSdk;
using MySqlConnector;
using System.Data;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// MySql仓储服务
    /// </summary>
    public partial class MySqlRepository<T> : DapperRepository<T>, IBaseAutofac where T : ITable
    {
        /// <summary>
        /// MySQL
        /// </summary>
        public override DatabaseType DatabaseType => DatabaseType.MySQL;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_MYSQL_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}