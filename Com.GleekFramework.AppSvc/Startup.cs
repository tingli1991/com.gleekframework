using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Com.GleekFramework.SwaggerSdk;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 程序激动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();//添加心跳
            services.AddKnife4Gen("测试文档");//添加Knife4生成器

            services.AddNewtonsoftJson();//添加对JSON的默认格式化
            services.AddDistributedMemoryCache();//添加分布式内存缓存
            services.AddGlobalExceptionAttribute();//添加全局异常
            services.AddModelValidAttribute<MessageCode>();//添加模型验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();//添加Cookie支持
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseNLogMiddleware();//添加日志记录器
            app.UseKnife4UI();//使用Knife4UI界面
            app.UseStaticFiles();//使用静态资源
            app.UseRouting();//使用路由规则
            app.UseHealthChecks();//使用心跳检测
            app.UseAuthentication();//启用授权
            app.UseEndpoints(endpoints => endpoints.MapControllers());//启用终结点配置
            app.RegisterApplicationStarted(async () =>
            {
                var queueClientService = app.ApplicationServices.GetRequiredService<QueueClientService>();
                await queueClientService.PublishAsync(MessageType.CUSTOMER_TEST_QUEUE_NAME);
                Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}");
            });
        }
    }
}