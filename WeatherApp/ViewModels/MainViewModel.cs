using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Views;

namespace WeatherApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _weatherService;
        private string _cityName = "New York";
        private string _currentTemp;
        private string _currentCondition;
        private string _currentIcon;
        private bool _isBusy;
        private string _searchText;
        private string _errorMessage;
        private double _currentLat = 40.7128;
        private double _currentLon = -74.0060;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _weatherService = new WeatherService();
            Forecast = new ObservableCollection<ForecastItem>();
            RefreshCommand = new Command(async () => await LoadWeatherDataAsync(_currentLat, _currentLon, _cityName));
            SearchCommand = new Command(async () => await SearchCityAsync());
            GoToDetailsCommand = new Command(async () => await GoToDetailsAsync());

            Task.Run(async () => await LoadWeatherDataAsync(_currentLat, _currentLon, "New York"));
        }

        public string CityName
        {
            get => _cityName;
            set { _cityName = value; OnPropertyChanged(); }
        }

        public string CurrentTemp
        {
            get => _currentTemp;
            set { _currentTemp = value; OnPropertyChanged(); }
        }

        public string CurrentCondition
        {
            get => _currentCondition;
            set { _currentCondition = value; OnPropertyChanged(); }
        }

        public string CurrentIcon
        {
            get => _currentIcon;
            set { _currentIcon = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public DateTime CurrentDate { get; } = DateTime.Now;

        public ObservableCollection<ForecastItem> Forecast { get; }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand GoToDetailsCommand { get; }

        private async Task SearchCityAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;

            IsBusy = true;
            ErrorMessage = null;

            var result = await _weatherService.GetCityCoordinatesAsync(SearchText);

            if (result == null)
            {
                ErrorMessage = $"City \"{SearchText}\" not found. Try again.";
                IsBusy = false;
                return;
            }

            var (lat, lon, cityName) = result.Value;
            _currentLat = lat;
            _currentLon = lon;
            await LoadWeatherDataAsync(lat, lon, cityName);
        }

        private async Task LoadWeatherDataAsync(double latitude, double longitude, string cityName)
        {
            IsBusy = true;

            try
            {
                var weather = await _weatherService.GetWeatherAsync(latitude, longitude);
                if (weather != null)
                {
                    CityName = cityName;
                    CurrentTemp = $"{weather.CurrentWeather.Temperature}°C";
                    CurrentCondition = _weatherService.GetWeatherCondition(weather.CurrentWeather.WeatherCode);
                    CurrentIcon = _weatherService.GetWeatherIcon(weather.CurrentWeather.WeatherCode);

                    Forecast.Clear();
                    for (int i = 0; i < weather.Daily.Time.Length; i++)
                    {
                        Forecast.Add(new ForecastItem
                        {
                            Date = DateTime.Parse(weather.Daily.Time[i]).ToString("ddd, MMM d"),
                            TempMax = weather.Daily.TemperatureMax[i],
                            TempMin = weather.Daily.TemperatureMin[i],
                            WeatherCondition = _weatherService.GetWeatherCondition(weather.Daily.WeatherCode[i]),
                            Icon = _weatherService.GetWeatherIcon(weather.Daily.WeatherCode[i])
                        });
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToDetailsAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new DetailsPage(_currentLat, _currentLon, _cityName));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}