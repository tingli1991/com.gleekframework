using NLog;

namespace Com.GleekFramework.NLogSdk
{
    /// <summary>
    /// 日志实现类
    /// </summary>
    public class NLogProvider
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly ILogger _log = LogManager.GetLogger(NLogConstant.GlobalLoggerName);

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Trace(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Trace, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Debug(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Debug, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Info(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Warn, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Warn(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Warn, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Error(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Error, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 写入致命性错误
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="serialNo">操作编号</param>
        /// <param name="url">接口路径</param>
        /// <param name="totalMilliseconds">程序执行时间(单位：毫秒)</param>
        public static void Fatal(string content, string serialNo = "", string url = "", long totalMilliseconds = 0)
            => Save(LogLevel.Fatal, new NLogModel() { SerialNo = serialNo, Content = content, Url = url, TotalMilliseconds = totalMilliseconds });

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="model"></param>
        public static void Save(LogLevel level, NLogModel model)
        {
            var logInfo = LogEventInfo.Create(level, _log.Name, model.Content);
            logInfo.Properties["Url"] = model.Url;
            logInfo.Properties["Content"] = model.Content;
            logInfo.Properties["SerialNo"] = model.SerialNo;
            logInfo.Properties["ServiceTime"] = model.ServiceTime;
            logInfo.Properties["ContentLength"] = model.ContentLength;
            logInfo.Properties["TotalMilliseconds"] = model.TotalMilliseconds;
            _log.Log(logInfo);
        }
    }
}