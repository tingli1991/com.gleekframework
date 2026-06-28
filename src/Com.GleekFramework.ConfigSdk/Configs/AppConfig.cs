using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// appsettings.json 配置文件
    /// </summary>
    public static partial class AppConfig
    {
        /// <summary>
        /// 配置文件信息
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public static IEnumerable<string> FileNames { get; set; }

        /// <summary>
        /// 使用appsetting.json配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAppConfig(IHostBuilder builder)
        {
            var fileNames = new string[]
            {
                ConfigConstant.BOOTSTRAP_CONFIG_FILENAME,//本地配置文件
                ConfigConstant.SUBSCRIPTION_CONFIG_FILENAME,//订阅配置文件
                ConfigConstant.APP_CONFIG_FILENAME,//应用程序配置文件
                ConfigConstant.SHARE_CONFIG_FILENAME,//共享配置文件
            };
            FileNames = ConfigPathProvider.GetEnvironmentFileNames(fileNames);//绑定文件路径
            Configuration = JsonConfigProvider.GetJsonConfiguration(ConfigConstant.DEFAULT_CONFIGURATION_NAME, FileNames.ToArray());//注册配置
            return builder;
        }
    }
}