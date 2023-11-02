namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 阿里云OSS
    /// </summary>
    public class AliyunOSSOptions
    {
        /// <summary>
        /// 上传/下载是使用的终结点地址
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// SDK AppID
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// App Key
        /// </summary>
        public string AccessSecret { get; set; }

        /// <summary>
        /// Bucket 域名
        /// </summary>
        public string BucketUrl { get; set; }
    }
}