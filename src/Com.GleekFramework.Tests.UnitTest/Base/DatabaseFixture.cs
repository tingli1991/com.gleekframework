using Com.GleekFramework.ConfigSdk;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Com.GleekFramework.UnitTest
{
    /// <summary>
    /// SQLite 测试数据库夹具，自动从配置加载连接字符串并初始化数据库
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        /// <summary>
        /// 共享 SQLite 连接
        /// </summary>
        public SqliteConnection Connection { get; private set; }

        /// <summary>
        /// 连接字符串（从配置文件读取）
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// 构造函数：加载配置、初始化数据库
        /// </summary>
        public DatabaseFixture()
        {
            // 确保配置系统已初始化
            if (AppConfig.Configuration == null)
            {
                Host.CreateDefaultBuilder()
                    .UseConfig()
                    .Build();
            }

            // 从配置读取 AuthCenterHosts 连接字符串
            ConnectionString = AppConfig.Configuration["ConnectionStrings:AuthCenterHosts"]
                ?? "Data Source=:memory:";

            Connection = new SqliteConnection(ConnectionString);
            Connection.Open();
            InitializeDatabase();
        }

        /// <summary>
        /// 初始化数据库表结构
        /// </summary>
        private void InitializeDatabase()
        {
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS test_product (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL DEFAULT 0,
                    Version INTEGER NOT NULL DEFAULT 1,
                    CreateTime TEXT NOT NULL DEFAULT '',
                    UpdateTime TEXT NOT NULL DEFAULT ''
                );
                CREATE TABLE IF NOT EXISTS test_order (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderNo TEXT NOT NULL,
                    TotalAmount REAL NOT NULL DEFAULT 0,
                    Status INTEGER NOT NULL DEFAULT 0,
                    CreateTime TEXT NOT NULL DEFAULT ''
                );";
            Connection.Execute(createTableSql);
        }

        /// <summary>
        /// 重置测试数据
        /// </summary>
        public void ResetData()
        {
            Connection.Execute("DELETE FROM test_product");
            Connection.Execute("DELETE FROM test_order");
            try { Connection.Execute("DELETE FROM sqlite_sequence"); } catch { }

            for (int i = 0; i < 3; i++)
            {
                Connection.Execute(
                    "INSERT INTO test_product (Name, Price, Version, CreateTime, UpdateTime) VALUES (@Name, @Price, @Version, datetime('now'), datetime('now'))",
                    new { Name = $"商品{(char)('A' + i)}", Price = 10.0 * (i + 1), Version = 1 });
            }
            Connection.Execute(
                "INSERT INTO test_order (OrderNo, TotalAmount, Status, CreateTime) VALUES (@OrderNo, @TotalAmount, @Status, datetime('now'))",
                new[] {
                    new { OrderNo = "ORD20240001", TotalAmount = 100.0, Status = 1 },
                    new { OrderNo = "ORD20240002", TotalAmount = 200.0, Status = 0 }
                });
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }

    /// <summary>
    /// 定义数据库测试集合
    /// </summary>
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
