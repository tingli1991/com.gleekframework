using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// ����Ԥ��
    /// </summary>
    public class WeatherForecastModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("date"), JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 摄氏度
        /// </summary>
        [JsonProperty("temperature_c"), JsonPropertyName("temperature_c")]
        public int TemperatureC { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 华氏度
        /// </summary>
        [JsonProperty("temperature_f"), JsonPropertyName("temperature_f")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}