using Com.GleekFramework.DapperSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Dapper
{
    /// <summary>
    /// Dapper SDK 类型单元测试
    /// </summary>
    public class DapperTypeTests : BaseUnitTest
    {
        /// <summary>
        /// DapperConstant 默认 MySQL 连接名不为空
        /// </summary>
        [Fact(DisplayName = "DapperConstantMySQL连接名不为空")]
        public void MySqlConnectionNameIsNotEmpty() =>
            Assert.False(string.IsNullOrWhiteSpace(DapperConstant.DEFAULT_MYSQL_CONNECTION_NAME));

        /// <summary>
        /// DapperConstant 默认 MSSQL 连接名不为空
        /// </summary>
        [Fact(DisplayName = "DapperConstantMSSQL连接名不为空")]
        public void MsSqlConnectionNameIsNotEmpty() =>
            Assert.False(string.IsNullOrWhiteSpace(DapperConstant.DEFAULT_MSSQL_CONNECTION_NAME));

        /// <summary>
        /// DapperRepository 泛型类型存在
        /// </summary>
        [Fact(DisplayName = "DapperRepository泛型类型存在")]
        public void DapperRepositoryTypeExists() =>
            Assert.NotNull(typeof(DapperRepository<>));

        /// <summary>
        /// MySqlRepository 泛型类型存在
        /// </summary>
        [Fact(DisplayName = "MySqlRepository泛型类型存在")]
        public void MySqlRepoTypeExists() =>
            Assert.NotNull(typeof(MySqlRepository<>));

        /// <summary>
        /// PgSqlRepository 泛型类型存在
        /// </summary>
        [Fact(DisplayName = "PgSqlRepository泛型类型存在")]
        public void PgSqlRepoTypeExists() =>
            Assert.NotNull(typeof(PgSqlRepository<>));
    }
}
