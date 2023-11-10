## 依赖注入SDK

### 约定/规范
1. 所有的依赖注入统一约定成属性注入的方式;
2. 利用属性注入的时候访问修饰符必须采用 `public` 关键字来进行修饰;
3. 所有定义的服务 `Service` 必须继承自 `IBaseAutofac` 接口(框架自认该接口的子类);
4. 在使用之前必须调用`HostingExtensions.cs`的`UseAutofac()`方法将Autofac注入到框架中,从而替换Net自带的IOC;
5. 所有定义的服务 `Service` 统一基于服务本身来进行注入(目的是提高开发效率，实际应用场景几乎不会出现需要换实现层的情况，如果非要加接口层，可以调整把`DefaultModule.cs`里面的`.AsSelf()`方法去掉即可)

### 启动注入示例
``` C#
/// <summary>
/// 程序类
/// </summary>
public static class Program
{
    /// <summary>
    /// 程序主函数
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        await CreateDefaultHostBuilder(args)
             .Build()
             .RunAsync();
    }

    /// <summary>
    /// 创建系统主机
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseAutofac()//启用Autofac依赖注入
        .UseGleekWebHostDefaults<Startup>();
}
```

#### 服务的定义
``` C#
/// <summary>
/// 程序集服务
/// </summary>
public partial class AssemblyService : IBaseAutofac
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>程序集</returns>
    public static Assembly GetAssembly(string assemblyName)
    {
        return AssemblyProvider.GetAssembly(assemblyName);
    }
}
```

#### 服务的使用
``` C#
using Com.GleekFramework.AssemblySdk;
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        /// 程序集服务
        /// </summary>
        public AssemblyService AssemblyService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult> ExecuteAsync()
        {
            var assemblyList = AssemblyService.GetAssemblyList();//获取执行目录下所有的程序集列表
            return await Task.FromResult(new ContractResult());
        }
    }
}
```
