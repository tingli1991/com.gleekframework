## 项目

> Com.GleekFramework.DapperSdk

## 依赖

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ContractSdk

## 概述

ORM(Dapper)拓展工具包。

- 支持通过特性自动映射 select 返回的列名称。
- 支持 MsSQL、MySQL、PgSQL、SQLite、Oracle。

### 相关仓储说明

- `MsSqlRepository` MsSql 仓储服务
- `MySqlRepository` MySql 仓储服务
- `SQLiteRepository` SQLite 数据仓储
- `PgSqlRepository` PgSQL 数据仓储
- `OracleRepository` Oracle 数据仓储

## 注入

`UseDapper()`和`UseDapperColumnMap()`是注入的核心方法。

- UseDapper() 用于注入数据库链接。
- UseDapperColumnMap() 再结合`Column`特性是为了解决 NET 模型的属性首字母大写，而数据库的字段是小写的映射问题。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.NacosSdk;
using Org.Gleek.AuthorizeSvc.Models;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 应用程序
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// 创建默认的主机
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseConfig()
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.AuthCenterHosts, DatabaseConstant.AuthCenterOnlyHosts)//注入mysql的连接字符串
            .UseDapperColumnMap("Org.Gleek.AuthorizeSvc.Entitys", "Org.Gleek.AuthorizeSvc.Models")//使用列的map映射
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.AuthCenterHosts)
            });
    }
}
```

## 示例

?> 其他的数据库用法一致，这里就以 MySQL 为例

### 定义用户模型

这里定义好用户模型后，直接用 [Migration 组件](/docs/zh-cn/components/MigrationSdk.md) 进行建表和字段的操作。

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Org.Gleek.AuthorizeSvc.Entitys
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("user")]
    [Comment("用户表")]
    [Index("idx_user_user_name", nameof(UserName))]
    public class User : VersionTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        [Comment("用户名")]
        [Column("user_name")]
        [JsonProperty("user_name"), JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        [Comment("密码")]
        [Column("password")]
        [JsonProperty("password"), JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(50)]
        [Comment("昵称")]
        [Column("nick_name")]
        [JsonProperty("nick_name"), JsonPropertyName("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(500)]
        [Comment("头像")]
        [Column("avatar")]
        [JsonProperty("avatar"), JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Comment("性别")]
        [Column("gender")]
        [DefaultValue(30)]
        [JsonProperty("gender"), JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }

        /// <summary>
        /// 个人名片
        /// </summary>
        [MaxLength(500)]
        [Comment("个人名片")]
        [Column("business_card")]
        [JsonProperty("business_card"), JsonPropertyName("business_card")]
        public string BusinessCard { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [MaxLength(1000)]
        [Comment("个性签名")]
        [Column("signature")]
        [JsonProperty("signature"), JsonPropertyName("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        [Column("is_admin")]
        [Comment("是否是超级管理员")]
        [JsonProperty("is_admin"), JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Comment("注册时间")]
        [Column("register_time", TypeName = "datetime")]
        [JsonProperty("register_time"), JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        [Comment("最近登录时间")]
        [Column("last_login_time", TypeName = "datetime")]
        [JsonProperty("last_login_time"), JsonPropertyName("last_login_time")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 最近登出时间
        /// </summary>
        [Comment("最近登出时间")]
        [Column("last_logout_time", TypeName = "datetime")]
        [JsonProperty("last_logout_time"), JsonPropertyName("last_logout_time")]
        public DateTime LastLogoutTime { get; set; }
    }
}
```

### 定义仓储

?> 只读仓储和读写仓储，区分是为了做读写分离，只是仓储名称和数据库连接不同，其他都是一致的，建议大家可以建立一个`BaseRepository`基类，将数据库访问仓储的示例可以放到该基类里面。

#### 读写仓储定义

```C#
using Com.GleekFramework.DapperSdk;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// 授权仓储(读写)
    /// </summary>
    public class AuthorizeRepository : MySqlRepository
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DatabaseConstant.AuthCenterHosts;
    }
}
```

#### 只读仓储定义

!> 注意：只读仓储的使用，一定是在业务能够接受延迟的情况下使用。

```C#
using Com.GleekFramework.DapperSdk;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// 授权仓储(只读)
    /// </summary>
    public class AuthorizeOnlyRepository : MySqlRepository
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => DatabaseConstant.AuthCenterOnlyHosts;
    }
}
```

### 定义基础仓储服务

```C#
using Com.GleekFramework.AutofacSdk;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// 基础仓储服务
    /// </summary>
    public class BaseRepository : IBaseAutofac
    {
        /// <summary>
        /// 认证授权仓储(读写)
        /// </summary>
        public AuthorizeRepository AuthorizeRepository { get; set; }

        /// <summary>
        /// 认证授权仓储(只读)
        /// </summary>
        public AuthorizeOnlyRepository AuthorizeOnlyRepository { get; set; }
    }
}
```

### 创建仓储服务

```C#
using Com.GleekFramework.CommonSdk;
using Org.Gleek.AuthorizeSvc.Entitys;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : BaseRepository
    {
        /// <summary>
        /// 获取登录授权参数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<JwtTokenModel> GetJwtTokenAsync(string userName)
        {
            if (userName.IsNull())
            {
                return null;
            }

            var sql = @"select id,user_name,nick_name,avatar,gender,business_card,is_admin,register_time,last_login_time,last_logout_time
            from user where user_name=@UserName and id_deleted=@IsDeleted";
            return await AuthorizeRepository.GetFirstOrDefaultAsync<JwtTokenModel>(sql, new { UserName = userName, IsDeleted = false });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string userName)
        {
            if (userName.IsNull())
            {
                return null;
            }

            var sql = @"select id,user_name,password,nick_name,avatar,gender,status,business_card,signature,is_admin,
            register_time,last_login_time,last_logout_time,version,id_deleted,update_time,create_time,extend,remark
            from user where user_name=@UserName and id_deleted=@IsDeleted";
            return await AuthorizeRepository.GetFirstOrDefaultAsync<User>(sql, new { UserName = userName, IsDeleted = false });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户名称</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(long userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            var sql = @"select id,user_name,password,nick_name,avatar,gender,status,business_card,signature,is_admin,
            register_time,last_login_time,last_logout_time,version,id_deleted,update_time,create_time,extend,remark
            from user where user_name=@Id and id_deleted=@IsDeleted";
            return await AuthorizeRepository.GetFirstOrDefaultAsync<User>(sql, new { Id = userId, IsDeleted = false });
        }
    }
}
```
