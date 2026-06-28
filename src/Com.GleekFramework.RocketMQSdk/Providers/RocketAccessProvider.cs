namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// Rocket账号基础信息
    /// </summary>
    public static partial class RocketAccessProvider
    {
        /// <summary>
        /// 账号配置信息
        /// </summary>
        public static RocketAccessOptions AccessOptions { get; set; }

        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="accessOptions">账号配置参数</param>
        public static void SetAccessOptions(RocketAccessOptions accessOptions)
        {
            AccessOptions = accessOptions;
        }
    }
}