using Com.GleekFramework.NLogSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 对象存储提供程序基类，封装公共逻辑
    /// </summary>
    public class StorageObject
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected static ObjectStorageOptions Options => ObjectStorageOptions.Instance;

        /// <summary>
        /// 处理包装器
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="func">要执行的异步方法</param>
        /// <returns>异步方法的执行结果</returns>
        protected static async Task<T> ExecuteAsync<T>(string bucketName, Func<string, Task<T>> func)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bucketName))
                {
                    bucketName = Options.DefaultBucketName;
                }
                return await func(bucketName);
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【对象存储】操作失败：{ex}");
                throw;
            }
        }
    }
}