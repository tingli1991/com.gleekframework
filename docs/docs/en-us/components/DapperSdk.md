## Project

> Com.GleekFramework.DapperSdk

## Dependencies

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ContractSdk

## Overview

ORM (Dapper) extension toolkit.

- Supports automatic mapping of column names returned by select through attributes.
- Supports MsSQL, MySQL, PgSQL, SQLite, Oracle.

### Related Repository Descriptions

- `MsSqlRepository` MsSql Repository Service
- `MySqlRepository` MySql Repository Service
- `SQLiteRepository` SQLite Data Repository
- `PgSqlRepository` PgSQL Data Repository
- `OracleRepository` Oracle Data Repository

## Injection

`UseDapper()` and `UseDapperColumnMap()` are the core methods for injection.

- UseDapper() is used to inject the database connection.
- UseDapperColumnMap() combined with the `Column` attribute is used to solve the mapping problem where the NET model's property starts with an uppercase letter while the database field is lowercase.

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
    /// Application
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create Default Host
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
            .UseDapper(DatabaseConstant.AuthCenterHosts, DatabaseConstant.AuthCenterOnlyHosts)//Inject mysql connection string
            .UseDapperColumnMap("Org.Gleek.AuthorizeSvc.Entitys", "Org.Gleek.AuthorizeSvc.Models")//Use column map mapping
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.AuthCenterHosts)
            });
    }
}
```

## Example

?> The usage for other databases is the same. Here we take MySQL as an example.

### Define User Model

After defining the user model here, use the Migration component for table and field operations.

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
    /// User Table
    /// </summary>
    [Table("user")]
    [Comment("User Table")]
    [Index("idx_user_user_name", nameof(UserName))]
    public class User : VersionTable
    {
        /// <summary>
        /// Username
        /// </summary>
        [MaxLength(50)]
        [Comment("Username")]
        [Column("user_name")]
        [JsonProperty("user_name"), JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [MaxLength(50)]
        [Comment("Password")]
        [Column("password")]
        [JsonProperty("password"), JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Nickname
        /// </summary>
        [MaxLength(50)]
        [Comment("Nickname")]
        [Column("nick_name")]
        [JsonProperty("nick_name"), JsonPropertyName("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        [MaxLength(500)]
        [Comment("Avatar")]
        [Column("avatar")]
        [JsonProperty("avatar"), JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [Comment("Gender")]
        [Column("gender")]
        [DefaultValue(30)]
        [JsonProperty("gender"), JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Comment("Status")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }

        /// <summary>
        /// Business Card
        /// </summary>
        [MaxLength(500)]
        [Comment("Business Card")]
        [Column("business_card")]
        [JsonProperty("business_card"), JsonPropertyName("business_card")]
        public string BusinessCard { get; set; }

        /// <summary>
        /// Signature
        /// </summary>
        [MaxLength(1000)]
        [Comment("Signature")]
        [Column("signature")]
        [JsonProperty("signature"), JsonPropertyName("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Is Super Admin
        /// </summary>
        [Column("is_admin")]
        [Comment("Is Super Admin")]
        [JsonProperty("is_admin"), JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Registration Time
        /// </summary>
        [Comment("Registration Time")]
        [Column("register_time", TypeName = "datetime")]
        [JsonProperty("register_time"), JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// Last Login Time
        /// </summary>
        [Comment("Last Login Time")]
        [Column("last_login_time", TypeName = "datetime")]
        [JsonProperty("last_login_time"), JsonPropertyName("last_login_time")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// Last Logout Time
        /// </summary>
        [Comment("Last Logout Time")]
        [Column("last_logout_time", TypeName = "datetime")]
        [JsonProperty("last_logout_time"), JsonPropertyName("last_logout_time")]
        public DateTime LastLogoutTime { get; set; }
    }
}
```

### Define Repository

?> Read-only repositories and read-write repositories are distinguished for read-write separation. They are only different in repository name and database connection, everything else is consistent. It's recommended to create a BaseRepository base class, where database access repository examples can be placed.

#### Read-Write Repository Definition

```C#
using Com.GleekFramework.DapperSdk;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// Authorization Repository (Read-Write)
    /// </summary>
    public class AuthorizeRepository : MySqlRepository
    {
        /// <summary>
        /// Configuration File Name
        /// </summary>
        public override string ConnectionName => DatabaseConstant.AuthCenterHosts;
    }
}
```

#### Read-Only Repository Definition

!> Note: The use of read-only repository is only under the condition that the business can accept delays.

```C#
using Com.GleekFramework.DapperSdk;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// Authorization Repository (Read-Only)
    /// </summary>
    public class AuthorizeOnlyRepository : MySqlRepository
    {
        /// <summary>
        /// Configuration File Name
        /// </summary>
        public override string ConnectionName => DatabaseConstant.AuthCenterOnlyHosts;
    }
}
```

### Define Base Repository Service

```C#
using Com.GleekFramework.AutofacSdk;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// Base Repository Service
    /// </summary>
    public class BaseRepository : IBaseAutofac
    {
        /// <summary>
        /// Authorization Repository (Read-Write)
        /// </summary>
        public AuthorizeRepository AuthorizeRepository { get; set; }

        /// <summary>
        /// Authorization Repository (Read-Only)
        /// </summary>
        public AuthorizeOnlyRepository AuthorizeOnlyRepository { get; set; }
    }
}
```

### Create Repository Service

```C#
using Com.GleekFramework.CommonSdk;
using Org.Gleek.AuthorizeSvc.Entitys;
using Org.Gleek.AuthorizeSvc.Models;

namespace Org.Gleek.AuthorizeSvc.Repository
{
    /// <summary>
    /// User Repository
    /// </summary>
    public class UserRepository : BaseRepository
    {
        /// <summary>
        /// Get Jwt Token
        /// </summary>
        /// <param name="userName">Username</param>
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
        /// Get User Info
        /// </summary>
        /// <param name="userName">Username</param>
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
        /// Get User Info
        /// </summary>
        /// <param name="userId">Username</param>
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
