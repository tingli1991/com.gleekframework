namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 接口地址常量
    /// </summary>
    public static partial class RestConstant
    {
        /// <summary>
        /// 获取配置接口
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetConfigsUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/cs/configs");

        /// <summary>
        /// 获取登录授权的Token接口
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static string GetAuthTokenUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/auth/login");

        /// <summary>
        /// 获取配置监听接口
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetConfigListenerUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/cs/configs/listener");

        /// <summary>
        /// 获取注册服务实例的接口地址
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetRegisterServiceInstanceUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/ns/instance");

        /// <summary>
        /// 获取注销服务实例的接口地址
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetRemoveServiceInstanceUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/ns/instance");

        /// <summary>
        /// 获取服务实例列表接口地址
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetServiceInstanceListUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/ns/instance/list");

        /// <summary>
        /// 获取发送服务实例心跳接口地址
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <returns></returns>
        public static string GetSendServiceInstanceBeatUrl(string baseUrl) => GetNacosApiUrl(baseUrl, "/nacos/v1/ns/instance/beat");

        /// <summary>
        /// 获取配置接口路径
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="action">接口路径</param>
        /// <returns></returns>
        private static string GetNacosApiUrl(string baseUrl, string action) => $"{baseUrl.TrimEnd('/')}/{action.TrimStart('/')}";
    }
}