namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 阿里云OSS
    /// </summary>
    public class AliyunOSSOptions
    {
        /// <summary>
        /// SDK AppID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// App Key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 地域节点
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Bucket 域名
        /// </summary>
        public string BucketUrl { get; set; }
    }
}