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