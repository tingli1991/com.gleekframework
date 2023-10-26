using Com.GleekFramework.AutofacSdk;
using MySql.Data.MySqlClient;
using System.Data;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// MySql仓储服务
    /// </summary>
    public partial class MySqlRepository : DapperRepository, IBaseAutofac
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_MYSQL_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}