namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 主机配置
    /// </summary>
    public class RabbitHostOptions
    {
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "guest";
    }
}