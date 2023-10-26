using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.NLogSdk
{
    /// <summary>
    /// 日志基础服务
    /// </summary>
    public partial class NLogService : IBaseAutofac
    {
        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Trace(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Trace(content, serialNo, url, totalMilliseconds);

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Debug(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Debug(content, serialNo, url, totalMilliseconds);

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Info(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Info(content, serialNo, url, totalMilliseconds);

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Warn(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Warn(content, serialNo, url, totalMilliseconds);

        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Error(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Error(content, serialNo, url, totalMilliseconds);

        /// <summary>
        /// 写入致命性错误
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public void Fatal(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => NLogProvider.Fatal(content, serialNo, url, totalMilliseconds);
    }
}