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
using Com.GleekFramework.RocketMQSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// ������
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .UseNacosService("org-gleek-fromework")
                 .SubscribeQueue()
                 //.SubscribeRocketMQ(config => config.Get<RocketConsumerOptions>("RocketConnectionOptions"))//����Rocket���ѷ���
                 .SubscribeRabbitMQ(config => config.Get<RabbitConsumerOptions>(Models.ConfigConstant.RabbitConnectionOptionsKey))//����RabbitMQ���ѷ���
                 .SubscribeKafka(config => config.Get<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))//����Kafka���ѷ���
                 .RunAsync();
        }

        /// <summary>
        /// ����ϵͳ����
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
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseDapperColumnMap("Com.GleekFramework.Models")
            .UseObjectStorage(config => config.Get<ObjectStorageOptions>("ObjectStorageOptions"))//ʹ�ö���洢����
            .AddRocketMQOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")) //���Rocket�˺�����
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}