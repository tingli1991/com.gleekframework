using Com.GleekFramework.AppSvc.Repositorys;
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;
namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// MySQL测试控制器
    /// </summary>
    [Route("mysql")]
    public class MySqlController : BaseController
    {
        /// <summary>
        /// 区域仓储
        /// </summary>
        public ComAreaRepository ComAreaRepository { get; set; }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        [HttpGet("pages")]
        public async Task<PageDataResult<ComArea>> GetPageListAsync(ComAreaPageParam param)
        {
            return await ComAreaRepository.GetPageListAsync(param);
        }
    }
}