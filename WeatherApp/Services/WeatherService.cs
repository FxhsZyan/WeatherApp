using System.Net.Http.Json;
using System.Text.Json.Serialization;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(double lat, double lon, string cityName)?> GetCityCoordinatesAsync(string city)
        {
            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GeocodingResponse>(url);
                if (response?.Results != null && response.Results.Length > 0)
                {
                    var result = response.Results[0];
                    return (result.Latitude, result.Longitude, $"{result.Name}, {result.Country}");
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true&daily=weathercode,temperature_2m_max,temperature_2m_min&timezone=auto";
            try
            {
                return await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
            }
            catch { return null; }
        }

        public async Task<WeatherDetails> GetWeatherDetailsAsync(double latitude, double longitude, string cityName)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}" +
             $"&hourly=relativehumidity_2m,visibility,uv_index&daily=uv_index_max&current_weather=true&timezone=auto&forecast_days=1";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<DetailedWeatherResponse>(url);
                if (response == null) return null;

                return new WeatherDetails
                {
                    CityName = cityName,
                    WindSpeed = response.CurrentWeather.WindSpeed,
                    // Take first hour values as "current"
                    Humidity = response.Hourly.Humidity != null && response.Hourly.Humidity.Length > 0
                        ? response.Hourly.Humidity[0] : 0,
                    Visibility = response.Hourly.Visibility != null && response.Hourly.Visibility.Length > 0
                        ? Math.Round(response.Hourly.Visibility[0] / 1000, 1) : 0,
                    // NEW
                    UVIndex = response.Hourly.UVIndex != null && response.Hourly.UVIndex.Length > 1
                    ? (int)response.Hourly.UVIndex[1] : 0,
                };
            }
            catch { return null; }
        }

        public string GetWeatherCondition(int code)
        {
            return code switch
            {
                0 => "Clear Sky",
                1 or 2 or 3 => "Partly Cloudy",
                45 or 48 => "Foggy",
                51 or 53 or 55 => "Drizzle",
                61 or 63 or 65 => "Rainy",
                71 or 73 or 75 => "Snowy",
                95 => "Thunderstorm",
                _ => "Unknown"
            };
        }

        public string GetWeatherIcon(int code)
        {
            return code switch
            {
                0 => "weather_icon_sunny.png",
                1 or 2 or 3 => "weather_icon_cloudy.png",
                61 or 63 or 65 or 51 or 53 or 55 => "weather_icon_rainy.png",
                _ => "weather_icon_cloudy.png"
            };
        }
    }

    // Geocoding models
    public class GeocodingResponse
    {
        [JsonPropertyName("results")]
        public GeocodingResult[] Results { get; set; }
    }

    public class GeocodingResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    // Detailed weather response models
    public class DetailedWeatherResponse
    {
        [JsonPropertyName("current_weather")]
        public CurrentWeather CurrentWeather { get; set; }

        [JsonPropertyName("hourly")]
        public HourlyDetails Hourly { get; set; }
    }

    public class HourlyDetails
    {
        [JsonPropertyName("relativehumidity_2m")]
        public int[] Humidity { get; set; }

        [JsonPropertyName("visibility")]
        public double[] Visibility { get; set; }

        [JsonPropertyName("uv_index")]
        public double[] UVIndex { get; set; }
    }
}