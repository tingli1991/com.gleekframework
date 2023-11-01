using System;
using System.Collections.Generic;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置基础常量
    /// </summary>
    public static class ConfigConstant
    {
        /// <summary>
        /// 系统默认的配置存放路径
        /// </summary>
        public const string DEFAULT_CONFIG = "Config";

        /// <summary>
        /// 共享JSON配置文件名称
        /// </summary>
        public const string SHARE_CONFIG_FILENAME = "share.json";

        /// <summary>
        /// 应用程序配置
        /// </summary>
        public const string APP_CONFIG_FILENAME = "application.json";

        /// <summary>
        /// 本地配置(独立配置)
        /// </summary>
        public const string BOOTSTRAP_CONFIG_FILENAME = "bootstrap.json";

        /// <summary>
        /// 订阅配置
        /// </summary>
        public const string SUBSCRIPTION_CONFIG_FILENAME = "subscription.json";

        /// <summary>
        /// 默认的配置文件名称
        /// </summary>
        public const string DEFAULT_CONFIGURATION_NAME = "default_configuration";

        /// <summary>
        /// 默认的配置文件存放路径
        /// </summary>
        public static readonly string DEFAULT_FILE_DIR = AppContext.BaseDirectory;

        /// <summary>
        /// 需要过滤掉的程序集名称
        /// </summary>
        public static readonly IEnumerable<string> FilterAssemblyNameList = new List<string>()
        {
            "NLog","Polly","System","Autofac","protobuf","NodaTime","Microsoft","AspNetCore",
            "Newtonsoft","Swashbuckle","Dapper","Npgsql","Log4Net","ValueInject","AutoMapper",
            "MySql.Data","SqlSugar","Quartz.Net","Lucene.Net","MySqlConnector","System.Data",
            "IBM.Data.DB2","Confluent.Kafka","MongoDB.Bson","MongoDB.Driver","ElasticSearch.Net",
            "RabbitMQ.Client","CSRedisCore","FluentMigrator","FluentMigrator","IGeekFan.AspNetCore",
            "StackExchange.Redis","Oracle.ManagedDataAccess",
        };
    }
}