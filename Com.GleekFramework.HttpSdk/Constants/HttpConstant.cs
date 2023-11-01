namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// Http常量配置
    /// </summary>
    public static class HttpConstant
    {
        /// <summary>
        /// 头部信息包含的白名单字符(通配)
        /// </summary>
        public const string WHITE_CONTAINS_STR = "-";

        /// <summary>
        /// 头部信息的流水编号
        /// </summary>
        public const string HEADER_SERIAL_NO_KEY = "x-serial-no";

        /// <summary>
        /// 默认的内容类型
        /// </summary>
        public const string CONTENT_TYPE_HEADER_NAME = "Content-Type";

        /// <summary>
        /// 默认的Http客户端名称
        /// </summary>
        public const string DEFAULT_CLIENT_NAME = "GEEK_FRAMEWORK_HTTP_CLIENT";
    }
}