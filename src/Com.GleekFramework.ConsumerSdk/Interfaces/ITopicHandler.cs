namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 主题分组处理类
    /// </summary>
    public interface ITopicHandler : IHandler
    {
        /// <summary>
        /// 定义排序编号
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 主题
        /// </summary>
        string Topic { get; }
    }
}