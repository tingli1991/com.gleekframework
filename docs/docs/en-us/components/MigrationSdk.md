## Project

> Com.GleekFramework.MigrationSdk

## Dependencies

- [Config Component](/docs/en-us/components/ConfigSdk.md)
- [Autofac Component](/docs/en-us/components/AutofacSdk.md)

## Overview

A database upgrade and migration toolkit, which is a secondary packaging based on `Dapper` and `FluentMigrator`.

- Supports automatic index creation.
- Supports automatic table and field creation.
- Can directly run SQL files to upgrade the database.
- Can directly run SQL scripts for database upgrades and other operations.
- Can perform data dimension repairs, initialization, and other operations through code.
- Currently only supports MySQL, MsSQL, PgSQL databases (MySQL has been tested, MsSQL, PgSQL are yet untested due to resource issues).

### Environment Variables

- MIGRATION_SWITCH Migration switch, the database will only be upgraded upon program startup if the switch is turned on.
- UPGRATION_SWITCH Upgrade switch, the database upgrade script will only be executed upon program startup if the switch is turned on.

### Definition of Migration and Upgrade

- Migration: Describes and acts upon database table structures, such as creating tables, fields, and indexes.
- Upgrade: Describes and acts upon database table data, such as initializing table data and deleting table data.

### Class Interface Description

- `ITable` interface: Defines the basic interface for data table models.

### Feature Explanation

- `Key` indicates primary key.
- `Index` signifies index.
- `Table` represents table name.
- `DefaultValue` for default values.
- `Column` denotes the table's column name.
- `MaxLength` for string length.
- `Comment` describes fields and tables.
- `DatabaseGenerated` for auto-increment attribute.

### Drawbacks

- Currently only supports MySQL, MsSQL, PgSQL databases; MsSQL, PgSQL yet untested due to resource issues.
- In a cluster deployment environment, if multiple server nodes start simultaneously, it could lead to errors or data duplicates (A temporary solution is to start one node in the cluster environment first, let the program run, and then start the remaining nodes).

## Injection

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
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create system host
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

## Migration

Tables, fields, and indexes are automatically created upon project startup once the model definitions are completed.

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
    /// Permissions table
    /// </summary>
    [Comment("Permissions table")]
    [Table("permission")]
    public class Permission : BasicTable
    {
        /// <summary>
        /// User name
        /// </summary>
        [MaxLength(50)]
        [Column("name")]
        [Comment("Role name")]
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Comment("Status")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }
    }
}
```

## Upgrade

!> Note: Upgrades have priorities, in descending order: `ExecuteScriptsAsync` > `ExecuteSqlFilesAsync` > `ExecuteAsync`, and SQL files can only be placed in the `Scripts` directory.

```C#
using Com.GleekFramework.MigrationSdk;

namespace Org.Gleek.AuthorizeSvc.Upgrations
{
    /// <summary>
    /// Regional code upgrade script
    /// </summary>
    [Upgration(1712500460000)]
    public class ComAreaUpgration : Upgration
    {
        /// <summary>
        /// Execute SQL files
        /// </summary>
        /// <returns></returns>
        public async override Task<IEnumerable<string>> ExecuteSqlFilesAsync()
        {
            return await Task.FromResult(new List<string>() { "com_area.sql" });
        }

        /// <summary>
        /// Execute SQL scripts
        /// </summary>
        /// <returns></returns>
        public override Task<IEnumerable<string>> ExecuteScriptsAsync()
        {
            return base.ExecuteScriptsAsync();
        }

        /// <summary>
        /// Execute code
        /// </summary>
        /// <returns></returns>
        public override Task ExecuteAsync()
        {
            return base.ExecuteAsync();
        }
    }
}
```
