using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// ���ò��Կ�����
    /// </summary>
    [Route("config")]
    public class ConfigController : BaseController
    {
        /// <summary>
        /// ע��������Ϣ
        /// </summary>
        [Config(Models.ConfigConstant.SummariesKey)]
        public static string[] Summaries { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Config(Models.ConfigConstant.ConfigOptionsKey)]
        public static ConfigOptions ConfitOptions { get; set; }

        /// <summary>
        /// ��������ע��
        /// </summary>
        public DependencyService DependencyService { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="param">�������</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContractResult> AddAsync(WeatherForecastParam param)
        {
            return await Task.FromResult(new ContractResult().SetSuceccful());
        }

        /// <summary>
        /// ��ȡ����Ԥ��
        /// </summary>
        /// <param name="appId">Id</param>
        /// <param name="status">״̬</param>
        /// <returns></returns>
        [HttpGet("{app_id}/{status}")]
        public async Task<IEnumerable<WeatherForecastModel>> GetAsync([RouteRequired("app_id", "������Id")] string appId, [RouteRequired("status", "������״̬")] bool status)
        {
            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }));
        }
    }
}