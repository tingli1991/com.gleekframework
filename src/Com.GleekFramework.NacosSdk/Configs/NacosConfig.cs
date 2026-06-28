using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos配置
    /// </summary>
    public static partial class NacosConfig
    {
        /// <summary>
        /// 配置文件信息
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public static IEnumerable<string> FileNames { get; set; }

        /// <summary>
        /// 使用独立的JSON配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseNacosConfig(IHostBuilder builder)
        {
            FileNames = ConfigPathProvider.GetEnvironmentFileNames(ConfigConstant.NACOS_CONFIG_FILENAME);//绑定文件路径
            Configuration = JsonConfigProvider.GetJsonConfiguration(ConfigConstant.NACOS_CONFIGURATION_NAME, FileNames.ToArray());//注册配置
            return builder;
        }
    }
}