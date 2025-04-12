using System;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 整个对象存储的配置项
    /// </summary>
    public class ObjectStorageOptions
    {
        /// <summary>
        /// 终结点地址
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 访问密钥
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 默认存储桶名称
        /// </summary>
        public string DefaultBucketName { get; set; }

        /// <summary>
        /// 静态实例，用于全局访问
        /// </summary>
        internal static ObjectStorageOptions Instance { get; private set; }

        /// <summary>
        /// 初始化静态配置
        /// </summary>
        /// <param name="options">配置项</param>
        internal static void Initialize(ObjectStorageOptions options)
        {
            Instance = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}