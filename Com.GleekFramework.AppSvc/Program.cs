using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.QueueSdk;

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
                 .UseMigrations((config) => new MigrationOptions()
                 {
                     MigrationSwitch = true,
                     UpgrationSwitch = true,
                     DatabaseType = DatabaseType.MySQL,
                     ConnectionString = config.GetConnectionString(DatabaseConstant.DefaultMySQLHostsKey)
                 })
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
            .UseGleekWebHostDefaults<Startup>();
    }
}