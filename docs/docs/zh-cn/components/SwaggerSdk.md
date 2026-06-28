## 项目

> Com.GleekFramework.SwaggerSdk

## 依赖

> Com.GleekFramework.AssemblySdk

> Com.GleekFramework.ConfigSdk

## 概述

Swagger 文档拓展工具包，它主要是利用 Knife4jUI 对 Swagger 进行的二次封装，使得我们的文档界面更加的美观，该组件使用很简单，是需要调用`SwaggerHostingExtensions`的`AddKnife4Gen()`和`UseKnife4UI()`方法即可完成。

!> 注意：由于 swagger 是基于项目的 xml 注释文件进行解析，所以我们在注入之前，还需要将相关的 `.csproj`文件怎加配置`<DocumentationFile>Com.GleekFramework.ConsumerSvc.xml</DocumentationFile>`，该配置在`PropertyGroup`节点下面，用于标识生成文档的注释文件。

### 环境变量

- **SWAGGER_SWITCH：** 用于标识 Swagger 的开关配置，只有打开 Swagger 文档才会被开启，有时候生产环境实际是不需要的，这种我们则可以将其关闭。

## 注入

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.SwaggerSdk;

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
            services.AddKnife4Gen("测试文档");//添加Knife4生成器
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseKnife4UI();//使用Knife4UI界面
            app.UseRouting();//使用路由规则
            app.UseEndpoints(endpoints => endpoints.MapControllers());//启用终结点配置
            app.RegisterApplicationStarted(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
        }
    }
}
```

### 效果

假设项目启动的端口配置的是 8080，那么打开 http://localhost:8080/ 即可跳转到 swagger 的文档页面。
<img src="images/swagger_ui.png" width="1920" height="580" alt="thanks" />
<img src="images/swagger_ui_details.png" width="1920" height="580" alt="thanks" />
