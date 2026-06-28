using Com.GleekFramework.ConfigSdk;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// Rabbit配置
    /// </summary>
    public class RabbitConfig
    {
        /// <summary>
        /// 默认的虚拟主机客户端配置
        /// </summary>
        [Config(ConfigConstant.RabbitDefaultClientHostsKey)]
        public static string RabbitDefaultClientHosts { get; set; }

        /// <summary>
        /// 默认的虚拟主机客户端配置
        /// </summary>
        [Config(ConfigConstant.KafkaDefaultClientHostsKey)]
        public static string KafkaDefaultClientHosts { get; set; }
    }
}