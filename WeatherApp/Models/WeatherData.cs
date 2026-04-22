using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("current_weather")]
        public CurrentWeather CurrentWeather { get; set; }

        [JsonPropertyName("daily")]
        public DailyWeather Daily { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("windspeed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("weathercode")]
        public int WeatherCode { get; set; }

        [JsonPropertyName("is_day")]
        public int IsDay { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }
    }

    public class DailyWeather
    {
        [JsonPropertyName("time")]
        public string[] Time { get; set; }

        [JsonPropertyName("weathercode")]
        public int[] WeatherCode { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public double[] TemperatureMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public double[] TemperatureMin { get; set; }
    }

    public class ForecastItem
    {
        public string Date { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public string WeatherCondition { get; set; }
        public string Icon { get; set; }
    }
}