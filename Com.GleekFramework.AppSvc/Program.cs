using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
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
                 .SubscribeRocketMQ(config => config.Get<RocketConsumerOptions>("RocketConnectionOptions"))//����Rocket���ѷ���
                 .SubscribeRabbitMQ(config => config.Get<RabbitConsumerOptions>(Models.ConfigConstant.RabbitConnectionOptionsKey))//����RabbitMQ���ѷ���
                 .SubscribeKafka(config => config.GetValue<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))//����Kafka���ѷ���
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
            .AddRocketMQAccessOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")) //���Rocket�˺�����
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}