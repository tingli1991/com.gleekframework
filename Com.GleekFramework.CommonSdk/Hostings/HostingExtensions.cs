using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 主机拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 启用信号两开关
        /// </summary>
        /// <param name="host"></param>
        /// <param name="minProcessorCount">最小能够处理请求的线程数(初始值)</param>
        /// <param name="maxProcessorCount">最大能够处理请求的线程数量(上限值)</param>
        /// <returns></returns>
        public static IHost UseSemaphoreSwitch(this IHost host, int minProcessorCount, int maxProcessorCount)
        {
            SemaphoreProvider.SetSemaphoreSwitch(minProcessorCount, maxProcessorCount);
            return host;
        }
    }
}