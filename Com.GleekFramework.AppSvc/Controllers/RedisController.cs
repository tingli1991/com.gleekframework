using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Redis控制器
    /// </summary>
    [Route("redis")]
    public class RedisController : BaseController
    {
        /// <summary>
        /// Redis哈希仓储类
        /// </summary>
        public RedisHashRepository RedisHashRepository { get; set; }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<ContractResult> TestAsync()
        {
            var isSuccess = await RedisHashRepository.SetAsync("hash_test", "hash_test_field", "hash_value", 500);
            isSuccess = await RedisHashRepository.SetNxAsync("hash_test_nx", "hash_test_field", "hash_value", 500);
            var number = await RedisHashRepository.IncrementAsync("hash_test_incr", "hash_test_field", 500);
            return await Task.FromResult(new ContractResult().SetSuceccful());
        }
    }
}
