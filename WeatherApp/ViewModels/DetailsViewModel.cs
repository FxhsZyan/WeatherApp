using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _weatherService;
        private string _cityName;
        private string _windSpeed;
        private string _humidity;
        private string _visibility;
        private string _uvIndex;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public DetailsViewModel()
        {
            _weatherService = new WeatherService();
        }

        public string CityName
        {
            get => _cityName;
            set { _cityName = value; OnPropertyChanged(); }
        }

        public string WindSpeed
        {
            get => _windSpeed;
            set { _windSpeed = value; OnPropertyChanged(); }
        }

        public string Humidity
        {
            get => _humidity;
            set { _humidity = value; OnPropertyChanged(); }
        }

        public string Visibility
        {
            get => _visibility;
            set { _visibility = value; OnPropertyChanged(); }
        }

        public string UVIndex
        {
            get => _uvIndex;
            set { _uvIndex = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public async Task LoadDetailsAsync(double latitude, double longitude, string cityName)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var details = await _weatherService.GetWeatherDetailsAsync(latitude, longitude, cityName);
                if (details != null)
                {
                    CityName = details.CityName;
                    WindSpeed = $"{details.WindSpeed} km/h";
                    Humidity = $"{details.Humidity}%";
                    Visibility = $"{details.Visibility} km";
                    UVIndex = $"{details.UVIndex}";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}