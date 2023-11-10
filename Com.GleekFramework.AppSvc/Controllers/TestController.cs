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