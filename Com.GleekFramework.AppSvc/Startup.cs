using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.Models;
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
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">当前注入的配置文件对象</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKnife4Gen("测试文档");//添加Knife4生成器
            services.AddNewtonsoftJson();//添加对JSON的默认格式化
            services.AddDistributedMemoryCache();//添加分布式内存缓存
            services.AddGlobalExceptionAttribute();//添加全局异常
            services.AddModelValidAttribute<MessageCode>();//添加模型验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();//添加Cookie支持
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="host"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHost host)
        {
            if (env.IsDevelopment())
            {
                //开发环境才需要出发的代码
                app.UseKnife4UI();//使用Knife4UI界面
                app.UseDeveloperExceptionPage();//使用开发人员异常页面
            }

            app.UseStaticFiles();//使用静态资源
            app.UseRouting();//使用路由规则
            //app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}