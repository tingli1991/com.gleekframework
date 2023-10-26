using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 信号量拓展实现类
    /// </summary>
    public static partial class SemaphoreProvider
    {
        /// <summary>
        /// 信号量开关
        /// </summary>
        private static bool @Switch { get; set; }

        /// <summary>
        /// 信号量(默认：CPU核心数量的2倍)
        /// </summary>
        private static SemaphoreSlim @SemaphoreSlim = new SemaphoreSlim(Environment.ProcessorCount * 2);

        /// <summary>
        /// 打开信号量开关配置
        /// </summary>
        /// <param name="minProcessorCount">最小能够处理请求的线程数(初始值)</param>
        /// <param name="maxProcessorCount">最大能够处理请求的线程数量(上限值)</param>
        public static void SetSemaphoreSwitch(int minProcessorCount, int maxProcessorCount)
        {
            if (minProcessorCount <= 0)
            {
                //默认使用进程数量
                minProcessorCount = Environment.ProcessorCount;
            }

            if (maxProcessorCount <= 0)
            {
                //默认使用进程数量*2
                minProcessorCount = Environment.ProcessorCount * 2;
            }
            @Switch = true;
            @SemaphoreSlim = new SemaphoreSlim(minProcessorCount, maxProcessorCount);
        }

        /// <summary>
        /// 执行信号量处理的方法
        /// </summary>
        /// <param name="callback">回调处理方法</param>
        /// <returns></returns>
        public static async Task ExecuteAsync(Func<int, Task> callback)
        {
            try
            {
                if (@Switch)
                {
                    //等待方法(获取信号量)
                    await @SemaphoreSlim.WaitAsync();
                }

                //执行回调方法
                await callback(@SemaphoreSlim.CurrentCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (@Switch)
                {
                    //释放信号量
                    @SemaphoreSlim.Release();
                }
            }
        }

        /// <summary>
        /// 执行信号量处理的方法
        /// </summary>
        /// <param name="callback">回调处理方法</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<int, Task<T>> callback)
        {
            try
            {
                if (@Switch)
                {
                    //等待方法(获取信号量)
                    await @SemaphoreSlim.WaitAsync();
                }

                //执行回调方法
                return await callback(@SemaphoreSlim.CurrentCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (@Switch)
                {
                    //释放信号量
                    @SemaphoreSlim.Release();
                }
            }
        }
    }
}