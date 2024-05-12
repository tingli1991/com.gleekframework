using System;
using System.Collections.Generic;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// 客户端配置选项
    /// </summary>
    public class HttpClientOptions
    {
        /// <summary>
        /// 超时时间(单位：秒，默认：3秒)
        /// </summary>
        public int TimeOutSeconds { get; set; } = 3;

        /// <summary>
        /// 等待并重试规则
        /// </summary>
        public List<TimeSpan> SleepDurations { get; set; }

        /// <summary>
        /// 最大连接数(默认：10000)
        /// </summary>
        public int MaxConnectionsPerServer { get; set; } = 10000;

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; } = HttpConstant.DEFAULT_CLIENT_NAME;

        /// <summary>
        /// 高级断路器
        /// </summary>
        public AdvancedCircuitBreakerOptions AdvancedCircuitBreakerOptions { get; set; }
    }

    /// <summary>
    /// 高级断路器
    /// </summary>
    public class AdvancedCircuitBreakerOptions
    {
        /// <summary>
        /// 采样持续时间(单位：秒，默认值：10秒)
        /// </summary>
        public int SamplingDuration { get; set; } = 10;

        /// <summary>
        /// 最小吞吐量(最少调用多少次，默认：10000)
        /// </summary>
        public int MinimumThroughput { get; set; } = 10000;

        /// <summary>
        /// 故障阈值(至少n%有异常则熔断，默认值：80%)
        /// </summary>
        public int FailureThreshold { get; set; } = 80;

        /// <summary>
        /// 中断持续时长(单位：秒，默认值：3秒)
        /// </summary>
        public int DurationOfBreak { get; set; } = 3;
    }
}