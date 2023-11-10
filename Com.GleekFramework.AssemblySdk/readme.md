#### 程序集开发工具包
|           服务名称                 |           服务描述                |              项目用途                                                                  |
|:-----------------------------------|:----------------------------------|:---------------------------------------------------------------------------------------|
| AssemblyService                    | 程序集服务                        |快速获取程序集信息                                                                      |
| DocumentService                    | 文档服务                          |快速获取文档信息(例如当前项目下的所有xml文件路径)                                       |
| LibraryService                     | 编译库服务                        |快速获取运行目录下的所有编译库名称列表                                                  |

#### 项目代码调用实例(以程序集服务为例)
``` C#
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
```