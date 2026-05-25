using System.Collections.ObjectModel;

namespace WeatherApp.ViewModels;

public class OnboardingSlide
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class OnboardingViewModel
{
    public ObservableCollection<OnboardingSlide> Slides { get; } = new()
    {
        new OnboardingSlide
        {
            Icon        = "weather_icon_sunny.png",
            Title       = "Welcome to WeatherApp",
            Description = "Your personal weather companion. Get real-time conditions and forecasts for any city in the world."
        },
        new OnboardingSlide
        {
            Icon        = "weather_icon_cloudy.png",
            Title       = "Search Any City",
            Description = "Type any city name in the search bar to instantly get live weather data, hourly forecasts, and a 7-day outlook."
        },
        new OnboardingSlide
        {
            Icon        = "weather_icon_wind.png",
            Title       = "Save Your Favorites",
            Description = "Tap the ♥ button to save cities you visit often. Access them quickly from the Favorites tab anytime."
        },
        new OnboardingSlide
        {
            Icon        = "weather_icon_rainy.png",
            Title       = "Track Your History",
            Description = "Every city you search is saved in History so you can revisit past searches without retyping."
        },
        new OnboardingSlide
        {
            Icon        = "weather_icon_uv.png",
            Title       = "Detailed Insights",
            Description = "Tap 'View Details' for in-depth info: UV index, humidity, wind speed, and visibility — all in one place."
        }
    };
}
