using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Net;
using System.Net.Http;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// 主机拓展
    /// </summary>
    public static partial class HttpHostingExtensions
    {
        /// <summary>
        /// 使用HttpClient
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="timeOutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public static IHostBuilder UseHttpClient(this IHostBuilder builder, int timeOutSeconds = 3)
        {
            builder.ConfigureServices(services => services.UseHttpClient(timeOutSeconds));
            return builder;
        }

        /// <summary>
        /// 使用Polly服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="timeOutSeconds">超时时间(单位：秒)</param>
        private static IServiceCollection UseHttpClient(this IServiceCollection services, int timeOutSeconds = 3)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient(HttpConstant.DEFAULT_CLIENT_NAME)
                .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(timeOutSeconds))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    UseProxy = false,
                    MaxAutomaticRedirections = 10,
                    MaxConnectionsPerServer = 10000,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                })).AddTransientHttpErrorPolicy(policy => policy.Or<CircuitException>().AdvancedCircuitBreakerAsync(
                    failureThreshold: 0.8,// 熔断高级配置，至少50%有异常则熔断
                    minimumThroughput: 1000,// 最少有多少次调用
                    durationOfBreak: TimeSpan.FromSeconds(30),// 断开时间（单位：秒）
                    samplingDuration: TimeSpan.FromSeconds(10),// 固定的时间范围内（单位：秒）
                    onReset: () => NLogProvider.Error("Circuit breaker reset"),//断路器复位
                    onHalfOpen: () => NLogProvider.Error("Circuit breaker in half-open state"),//断路器处于半开状态
                    onBreak: (context, timespan) => NLogProvider.Error($"Circuit breaker opened for {timespan.TotalMilliseconds} milliseconds due to {context.Exception}")//断路器被打开的事件
                ));
            return services;
        }
    }
}