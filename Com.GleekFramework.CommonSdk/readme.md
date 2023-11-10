## 基础开发工具包
此SDK作为整个框架最基础的一个工具包，它主要帮我们定义了一些最最基础的自定义特性/枚举/拓展方法/信号量(控制异步线程数量使用)/模型转换/数据验证/以及一些基础的反射(优化过的)以及常规的数据验证等等功能

### 约定/规范
1. 该项目几乎所有的`class`文件都是静态类，所有的方法几乎都是静态方法和拓展方法;
2. 该项目的所有方法都可以像访问静态方法(或者拓展方法)的方式进行调用(推荐使用拓展方法的访问方式进行调用);
3. 以上2点仅限于绝大部分文件(例如：特性，和枚举)比较特殊，他们只是一些公用的定义而已;


#### 基础调用示例
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
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
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult> ExecuteAsync()
        {
            //常规的验证代码
            var idcar = "544094199101115891".IsCardId();//验证是否是省份证号码
            var isNumber = "ssss111".IsNumber();//验证是否是数字
            var isJson = "{}".IsJson();//验证是否是Json数据
            var isBool = "false".IsBool();//是否是bool类型
            var isEmail = "899899@qq.com".IsEmail();//是否是邮件

            
            //单个模型的转换
            var sourceInfo = new ClassA() { Id = 1, Name = "张三" };
            var toInfo = sourceInfo.Map<ClassA, ClassB>();

            //集合类型转换
            var sourceList = new List<ClassA>()
            {
                new ClassA() {Id=1,Name="张三"},
                new ClassA() {Id=2,Name="里斯"},
            };
            var toSourceList = sourceList.Map<ClassA, ClassB>();

            return await Task.FromResult(new ContractResult());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClassA
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 目标类
    /// </summary>
    public class ClassB
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
``` 