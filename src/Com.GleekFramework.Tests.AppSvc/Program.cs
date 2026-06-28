using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.ObjectSdk;
using Com.GleekFramework.QueueSdk;
using Com.GleekFramework.RabbitMQSdk;
using Com.GleekFramework.RedisSdk;
using Com.GleekFramework.RocketMQSdk;

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
                 .UseNacosService("org-gleek-fromework")
                 .SubscribeQueue()
                 .SubscribeStack()
                 //.SubscribeRocketMQ(config => config.Get<RocketConsumerOptions>("RocketConnectionOptions"))//订阅Rocket消费服务
                 .SubscribeRabbitMQ(config => config.Get<RabbitConsumerOptions>(Models.ConfigConstant.RabbitConnectionOptionsKey))//订阅RabbitMQ消费服务
                 .SubscribeKafka(config => config.Get<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))//订阅Kafka消费服务
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
            .UseCsRedis()
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseDapperColumnMap("Com.GleekFramework.Models")
            .UseObjectStorage(config => config.Get<ObjectStorageOptions>("ObjectStorageOptions"))//使用对象存储配置
            .AddRocketMQOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")) //添加Rocket账号配置
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}