using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.QueueSdk;
using Com.GleekFramework.DapperSdk;

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
                 .SubscribeStack((config) => 24)//���ı���ջ(�Ƚ��Գ�)
                 .SubscribeQueue((config) => 24)//���ı��ض���(�Ƚ����)
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
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.GetValue(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}