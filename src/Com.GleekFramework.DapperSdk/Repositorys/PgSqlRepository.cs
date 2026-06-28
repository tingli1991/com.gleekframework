using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ContractSdk;
using Npgsql;
using System.Data;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// PgSQL数据仓储
    /// 连接字符串格式：User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;
    /// </summary>
    public partial class PgSqlRepository<T> : DapperRepository<T>, IBaseAutofac where T : ITable
    {
        /// <summary>
        /// PgSQL
        /// </summary>
        public override DatabaseType DatabaseType => DatabaseType.PgSQL;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_PGSQL_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override IDbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}