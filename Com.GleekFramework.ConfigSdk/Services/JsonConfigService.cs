using Com.GleekFramework.AutofacSdk;
using Microsoft.Extensions.Configuration;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// JSON配置基础能力提供者
    /// </summary>
    public partial class JsonConfigService : IBaseAutofac
    {
        /// <summary>
        /// 获取JSON配置信息
        /// </summary>
        /// <param name="configurationName">配置文件名称</param>
        /// <returns></returns>
        public IConfiguration GetConfiguration(string configurationName = ConfigConstant.DEFAULT_CONFIGURATION_NAME)
            => JsonConfigProvider.GetConfiguration(configurationName);
    }
}