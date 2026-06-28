## 项目

> Com.GleekFramework.MigrationSdk

## 依赖

- [Config 组件](/docs/zh-cn/components/ConfigSdk.md)
- [Autofac 组件](/docs/zh-cn/components/AutofacSdk.md)

## 概述

版本迁移拓展工具包，他是基于`Dapper`和`FluentMigrator`进行二次封装的数据库升级和迁移工具包。

- 支持自动建索引。
- 支持自动建表、建字段。
- 可以直接运行 sql 文件，对数据库进行升级操作。
- 可以直接运行 sql 脚本，对数据库进行升级等操作。
- 可以通过代码的方式执行数据维度的修复、初始化等操作。
- 目前只支持 MySQL、MsSQL、PgSQL 数据库(亲测 MySQL，MsSQL、PgSQL 由于资源问题目前还未测试)。

### 环境变量

- MIGRATION_SWITCH 迁移开关，只有开关被打开，程序启动的时候才会执行数据库的升级。
- UPGRATION_SWITCH 升级开关，只有开关被打开，程序启动的时候才会执行数据库升级脚本。

### 迁移和升级定义

- 迁移：是对数据库表结构方面的描述和动作，例如：建表、建字段、建索引等。
- 升级：是对数据库表数据方面的描述和动作，例如：初始化表数据、删除表数据等。

### 类接口说明

- `ITable`接口：定义数据表模型的基础接口

### 特性说明

- `Key` 标识主键。
- `Index` 标识索引。
- `Table` 标识表名称。
- `DefaultValue` 默认值。
- `Column` 用于表示表的列名称。
- `MaxLength` 用于表示字符串长度。
- `Comment` 用于表示字段和表的描述。
- `DatabaseGenerated` 自增特性。

### 缺陷

- 目前只支持 MySQL、MsSQL、PgSQL 数据库，而且 MsSQL、PgSQL 由于资源问题目前还未测试。
- 在部署集群环境的时候，如果多个服务器节点同时启动，会导致报错，或者数据重复等问题(该问题带解决，临时解决方案是集群环境启动的时候先启动一个节点，等程序运行起来之后，再启动剩余的节点)。

## 注入

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 程序类
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 程序主函数
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// 创建系统主机
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseConfig()
            .UseNacosConf()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

## 迁移

模型定义完成之后，项目启动则会自动建表建字段以及索引。

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 权限表
    /// </summary>
    [Comment("权限表")]
    [Table("permission")]
    public class Permission : BasicTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        [Column("name")]
        [Comment("角色名称")]
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }
    }
}
```

## 升级

!> 注意：升级有优先级，他们的优先级分别是`ExecuteScriptsAsync` > `ExecuteSqlFilesAsync` > `ExecuteAsync`，同时 sql 文件只能放在`Scripts`目录下面。

```C#
using Com.GleekFramework.MigrationSdk;

namespace Org.Gleek.AuthorizeSvc.Upgrations
{
    /// <summary>
    /// 地区编码升级脚本
    /// </summary>
    [Upgration(1712500460000)]
    public class ComAreaUpgration : Upgration
    {
        /// <summary>
        /// 执行sql文件
        /// </summary>
        /// <returns></returns>
        public async override Task<IEnumerable<string>> ExecuteSqlFilesAsync()
        {
            return await Task.FromResult(new List<string>() { "com_area.sql" });
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <returns></returns>
        public override Task<IEnumerable<string>> ExecuteScriptsAsync()
        {
            return base.ExecuteScriptsAsync();
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        /// <returns></returns>
        public override Task ExecuteAsync()
        {
            return base.ExecuteAsync();
        }
    }
}
```
