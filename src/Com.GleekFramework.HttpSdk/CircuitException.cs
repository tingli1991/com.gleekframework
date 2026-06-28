using System;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// 自定义熔断异常
    /// </summary>
    public class CircuitException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public CircuitException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CircuitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}