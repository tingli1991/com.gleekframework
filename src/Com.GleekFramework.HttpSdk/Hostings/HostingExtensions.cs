using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.Configuration;
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
            return builder.ConfigureServices(services => services.UseHttpClient(timeOutSeconds));
        }

        /// <summary>
        /// 使用Polly服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="timeOutSeconds">超时时间(单位：秒)</param>
        private static IServiceCollection UseHttpClient(this IServiceCollection services, int timeOutSeconds = 3)
        {
            return services.UseHttpClient(config => new HttpClientOptions() { TimeOutSeconds = timeOutSeconds });
        }

        /// <summary>
        /// 使用HttpClient
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="callback">超时时间(单位：秒)</param>
        /// <returns></returns>
        public static IHostBuilder UseHttpClient(this IHostBuilder builder, Func<IConfiguration, HttpClientOptions> callback)
        {
            return builder.ConfigureServices(services => services.UseHttpClient(callback));
        }

        /// <summary>
        /// 使用HttpClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="callback"></param>
        public static IServiceCollection UseHttpClient(this IServiceCollection services, Func<IConfiguration, HttpClientOptions> callback)
        {
            services.AddHttpContextAccessor();
            var options = callback(AppConfig.Configuration);//获取配置
            var builder = services.AddHttpClient(options.ClientName);//绑定客户端
            builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerServer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            if (options.TimeOutSeconds > 0)
            {
                //添加超时时间(单位：秒)
                builder.ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(options.TimeOutSeconds));
            }

            if (options.SleepDurations.IsNotNull())
            {
                //添加等待间隔重试规则
                builder.AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(options.SleepDurations));
            }

            if (options.AdvancedCircuitBreakerOptions.IsNotNull())
            {
                //添加高级断路器(断路器配置请谨慎添加)
                builder.AddTransientHttpErrorPolicy(policy => policy.Or<CircuitException>().AdvancedCircuitBreakerAsync(
                    failureThreshold: options.AdvancedCircuitBreakerOptions.FailureThreshold / 100,// 熔断高级配置，至少80%有异常则熔断
                    minimumThroughput: options.AdvancedCircuitBreakerOptions.MinimumThroughput,// 最少有多少次调用
                    durationOfBreak: TimeSpan.FromSeconds(options.AdvancedCircuitBreakerOptions.DurationOfBreak),// 断开时间（单位：秒）
                    samplingDuration: TimeSpan.FromSeconds(options.AdvancedCircuitBreakerOptions.SamplingDuration),// 固定的时间范围内（单位：秒）
                    onReset: () => NLogProvider.Error("Circuit breaker reset"),//断路器复位
                    onHalfOpen: () => NLogProvider.Error("Circuit breaker in half-open state"),//断路器处于半开状态
                    onBreak: (context, timespan) => NLogProvider.Error($"Circuit breaker opened for {timespan.TotalMilliseconds} milliseconds due to {context.Exception}")//断路器被打开的事件
                ));
            }
            return services;
        }
    }
}