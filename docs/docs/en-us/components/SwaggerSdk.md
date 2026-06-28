## Project

> Com.GleekFramework.SwaggerSdk

## Dependencies

> Com.GleekFramework.AssemblySdk

> Com.GleekFramework.ConfigSdk

## Overview

A Swagger documentation extension toolkit, it mainly uses Knife4jUI for the secondary packaging of Swagger to make our documentation interface more beautiful. The component is very easy to use, you just need to call `SwaggerHostingExtensions`'s `AddKnife4Gen()` and `UseKnife4UI()` methods to complete the process.
!> Note: Since Swagger analyzes based on the xml comment file of the project, before injecting, we also need to add the configuration `<DocumentationFile>Com.GleekFramework.ConsumerSvc.xml</DocumentationFile>` to the .csproj file. This configuration is under the `PropertyGroup` node and identifies the comment file that generates the documentation.

### Environment Variables

- **SWAGGER_SWITCH：** SWAGGER_SWITCH is used to indicate the Swagger switch configuration. The Swagger documentation will only be enabled if the Swagger switch is turned on. Sometimes it is not needed in the production environment and can be closed off.

## Injection

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.SwaggerSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program activation class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Service registration
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKnife4Gen("Test Documentation");//Add Knife4 generator
        }

        /// <summary>
        /// Configuration service
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseKnife4UI();//Use the Knife4UI interface
            app.UseRouting();//Use routing rules
            app.UseEndpoints(endpoints => endpoints.MapControllers());//Enable endpoint configuration
            app.RegisterApplicationStarted(() => Console.Out.WriteLine($"Service started successfully: {EnvironmentProvider.GetHost()}"));
        }
    }
}
```

## Effects

Assuming the project's launch port is configured as 8080, opening `http://localhost:8080/` will redirect you to the Swagger documentation page.
<img src="images/swagger_ui.png" width="1920" height="580" alt="thanks" />
<img src="images/swagger_ui_details.png" width="1920" height="580" alt="thanks" />
