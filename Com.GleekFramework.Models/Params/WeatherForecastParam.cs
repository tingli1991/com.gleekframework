using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class WeatherForecastParam
    {
        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("date"), JsonPropertyName("date")]
        [Required(ErrorMessage = nameof(MessageCode.PARAM_REQUIRED_DATE))]
        public DateTime Date { get; set; }

        /// <summary>
        /// 温度(摄氏度)
        /// </summary>
        [JsonProperty("temperature_c"), JsonPropertyName("temperature_c")]
        [Required(ErrorMessage = nameof(GlobalMessageCode.PARAM_REQUIRED))]
        public int TemperatureC { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; }

        /// <summary>
        /// 温度(华氏度)
        /// </summary>
        [JsonProperty("temperature_f"), JsonPropertyName("temperature_f")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}