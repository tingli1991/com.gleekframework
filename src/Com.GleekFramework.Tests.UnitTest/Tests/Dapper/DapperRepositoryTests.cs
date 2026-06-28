using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.DapperSdk;
using Dapper;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Dapper
{
    /// <summary>
    /// Dapper 仓储 SQLite 集成测试
    /// </summary>
    [Collection("DatabaseCollection")]
    public class DapperRepositoryTests : BaseUnitTest
    {
        private readonly DatabaseFixture _fixture;

        /// <summary>
        /// 构造函数，自动注入已初始化的数据库
        /// </summary>
        public DapperRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetData();
        }

        /// <summary>
        /// SQLiteRepository 的 ConnectionName 和 DatabaseType 正确
        /// </summary>
        [Fact(DisplayName = "SQLiteRepository使用SQLite连接名")]
        public void SqliteRepositoryConnectionName()
        {
            var repo = new TestSqliteRepository(_fixture.ConnectionString);
            Assert.Equal(DapperConstant.DEFAULT_SQLLITE_CONNECTION_NAME, repo.ConnectionName);
            Assert.Equal(DatabaseType.SQLite, repo.DatabaseType);
        }

        /// <summary>
        /// 通过共享连接查询数据
        /// </summary>
        [Fact(DisplayName = "查询列表")]
        public void QueryList()
        {
            var list = _fixture.Connection.Query<TestProductModel>("SELECT * FROM test_product");
            Assert.Equal(3, list.Count());
        }

        /// <summary>
        /// 通过共享连接查询单条
        /// </summary>
        [Fact(DisplayName = "查询单条")]
        public void QuerySingle()
        {
            var item = _fixture.Connection.QueryFirstOrDefault<TestProductModel>(
                "SELECT * FROM test_product WHERE Name = @Name", new { Name = "商品B" });
            Assert.NotNull(item);
            Assert.Equal("商品B", item.Name);
        }

        /// <summary>
        /// 通过共享连接插入数据
        /// </summary>
        [Fact(DisplayName = "插入数据")]
        public void Insert()
        {
            _fixture.Connection.Execute(
                "INSERT INTO test_product (Name, Price, Version, CreateTime, UpdateTime) VALUES (@Name, @Price, @Version, datetime('now'), datetime('now'))",
                new { Name = "新商品", Price = 199.0, Version = 1 });
            Assert.Equal(4, _fixture.Connection.ExecuteScalar<int>("SELECT COUNT(1) FROM test_product"));
        }

        /// <summary>
        /// 聚合查询
        /// </summary>
        [Fact(DisplayName = "聚合查询")]
        public void Aggregate()
        {
            Assert.Equal(3, _fixture.Connection.ExecuteScalar<int>("SELECT COUNT(1) FROM test_product"));
        }

        /// <summary>
        /// 异步查询
        /// </summary>
        [Fact(DisplayName = "异步查询")]
        public async Task QueryAsync()
        {
            var count = await _fixture.Connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM test_product");
            Assert.Equal(3, count);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        [Fact(DisplayName = "批量更新")]
        public void BatchUpdate()
        {
            _fixture.Connection.Execute("UPDATE test_product SET Price = @Price WHERE Name = @Name", new[] {
                new { Name = "商品A", Price = 50.0 },
                new { Name = "商品B", Price = 100.0 }
            });
            Assert.Equal(180.0, _fixture.Connection.ExecuteScalar<double>("SELECT SUM(Price) FROM test_product"));
        }
    }

    /// <summary>
    /// 测试 SQLite 仓储
    /// </summary>
    public class TestSqliteRepository : DapperRepository<TestProductModel>
    {
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestSqliteRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public override DatabaseType DatabaseType => DatabaseType.SQLite;

        /// <summary>
        /// 连接名
        /// </summary>
        public override string ConnectionName => DapperConstant.DEFAULT_SQLLITE_CONNECTION_NAME;

        /// <summary>
        /// 获取连接
        /// </summary>
        protected override IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }

    /// <summary>
    /// 测试商品模型
    /// </summary>
    public class TestProductModel : ITable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [Column("Version")] public decimal Version { get; set; }
        [Column("CreateTime")] public DateTime CreateTime { get; set; }
        [Column("UpdateTime")] public DateTime UpdateTime { get; set; }
    }
}
