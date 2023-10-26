using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 依赖注入生成器
    /// </summary>
    public static partial class NacosProvider
    {
        /// <summary>
        /// Nacos系统配置信息
        /// </summary>
        public static NacosSettings Settings = null;

        /// <summary>
        /// 添加Nacos
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseNacosConf(IHostBuilder builder)
        {
            Settings = NacosConfig.Configuration.GetConfiguration<NacosSettings>();
            if (Settings.Switch)
            {
                Settings.BatchInitNacos();//批量初始化配置
                Settings.BatchAddListener();//批量添加监听
            }
            return builder;
        }

        /// <summary>
        /// 使用阿里巴巴的Nacos配置功能
        /// </summary>
        /// <param name="host"></param>
        /// <param name="serverName">服务名称</param>
        public static IHost UseNacosService(IHost host, string serverName)
        {
            if (Settings == null)
            {
                throw new ArgumentNullException("nacosConf");
            }

            if (!Settings.Switch)
            {
                return host;
            }

            var lifeTime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(async () =>
            {
                await Settings.RegisterInstanceAsync(serverName);//注册实例
                await Settings.StartServiceInstanceBeatCheckAsync(serverName);//启动实例的心跳检查
            });

            lifeTime.ApplicationStopping.Register(async () =>
            {
                //注销服务实例
                await Settings.RemoveServiceInstanceAsync(serverName);
            });

            lifeTime.ApplicationStopped.Register(() =>
            {

            });
            return host;
        }
    }
}