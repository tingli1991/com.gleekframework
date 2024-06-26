﻿using Com.GleekFramework.AutofacSdk;
using System.Data;
using System.Data.SQLite;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SQLite数据仓储
    /// </summary>
    public partial class SQLiteRepository : DapperRepository, IBaseAutofac
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_SQLLITE_CONNECTION_NAME;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
}