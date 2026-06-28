namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// Rocket基础配置
    /// </summary>
    public class RocketAccessOptions
    {
        /// <summary>
        /// AccessKey Secret阿里云身份验证，在阿里云用户信息管理控制台创建。
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// AccessKey ID阿里云身份验证，在阿里云用户信息管理控制台创建。
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 若实例有命名空间，则实例ID必须传入；若实例无命名空间，则实例ID传入null空值或字符串空值。实例的命名空间可以在消息队列RocketMQ版控制台的实例详情页面查看。
        /// </summary>
        public string InstanceId { get; set; }
    }
}