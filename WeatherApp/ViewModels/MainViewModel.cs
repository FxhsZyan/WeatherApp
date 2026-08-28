using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using Microsoft.Maui.ApplicationModel;


namespace WeatherApp.ViewModels
{
    [QueryProperty(nameof(IncomingLat), "lat")]
    [QueryProperty(nameof(IncomingLon), "lon")]
    [QueryProperty(nameof(IncomingCity), "city")]
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _weatherService;
        private readonly DatabaseService _databaseService;

        private string _cityName = "New York";
        private string _currentTemp;
        private string _currentCondition;
        private string _currentIcon;
        private string _searchText;
        private bool _isBusy;
        private bool _isFavorite;
        private double _currentLat = 40.7128;
        private double _currentLon = -74.0060;
        private string _aiSummary;
        private bool _isAiSummaryLoading;



        public event PropertyChangedEventHandler PropertyChanged;



        public MainViewModel()
        {
            _weatherService = new WeatherService();
            _databaseService = new DatabaseService();
            Forecast = new ObservableCollection<ForecastItem>();
            HourlyForecast = new ObservableCollection<HourlyForecastItem>();

            RefreshCommand = new Command(async () => await LoadWeatherDataAsync(_currentLat, _currentLon));
            SearchCommand = new Command(async () => await SearchCityAsync());
            ToggleFavoriteCommand = new Command(async () => await ToggleFavoriteAsync());
            ViewDetailsCommand = new Command(async () => await ViewDetailsAsync());

            Task.Run(async () => await LoadLastCityOrDefaultAsync());
        }

        public string IncomingLat
        {
            set
            {
                if (double.TryParse(value, out double lat))
                    _currentLat = lat;
            }
        }

        public string IncomingLon
        {
            set
            {
                if (double.TryParse(value, out double lon))
                    _currentLon = lon;
            }
        }

        public string IncomingCity
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CityName = Uri.UnescapeDataString(value);
                    MainThread.BeginInvokeOnMainThread(async () =>
                        await LoadWeatherDataAsync(_currentLat, _currentLon));
                }
            }
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

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set { _isFavorite = value; OnPropertyChanged(); }
        }

        public string AiSummary
        {
            get => _aiSummary;
            set { _aiSummary = value; OnPropertyChanged(); }
        }

        public bool IsAiSummaryLoading
        {
            get => _isAiSummaryLoading;
            set { _isAiSummaryLoading = value; OnPropertyChanged(); }
        }

        public DateTime CurrentDate { get; } = DateTime.Now;

        public ObservableCollection<ForecastItem> Forecast { get; }

        public ObservableCollection<HourlyForecastItem> HourlyForecast { get; }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        private async Task SearchCityAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var result = await _weatherService.GetCityCoordinatesAsync(SearchText);
            if (result == null)
            {
                await Application.Current.MainPage.DisplayAlert("Not Found", $"Could not find city: {SearchText}", "OK");
                return;
            }

            _currentLat = result.Value.lat;
            _currentLon = result.Value.lon;
            CityName = result.Value.cityName;
            SearchText = string.Empty;
            // Save last searched city
            Preferences.Set("last_city", CityName);
            Preferences.Set("last_lat", _currentLat);
            Preferences.Set("last_lon", _currentLon);
            Preferences.Set("last_city_name", CityName);

            await LoadWeatherDataAsync(_currentLat, _currentLon);

            // Save to history
            await _databaseService.AddHistoryAsync(new SearchHistory
            {
                CityName = CityName,
                Latitude = _currentLat,
                Longitude = _currentLon,
                SearchedAt = DateTime.Now
            });

            // Check if already favorite
            IsFavorite = await _databaseService.IsFavoriteAsync(CityName);
        }

        private async Task LoadWeatherDataAsync(double lat, double lon)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var weather = await _weatherService.GetWeatherAsync(lat, lon);
                if (weather != null)
                {
                    CurrentTemp = $"{weather.CurrentWeather.Temperature}°C";
                    CurrentCondition = _weatherService.GetWeatherCondition(weather.CurrentWeather.WeatherCode);
                    CurrentIcon = _weatherService.GetWeatherIcon(weather.CurrentWeather.WeatherCode);

                    // 7-day forecast
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

                    // Hourly forecast — show next 8 hours from current hour
                    HourlyForecast.Clear();
                    if (weather.Hourly != null)
                    {
                        var now = DateTime.Now;
                        int count = 0;
                        for (int i = 0; i < weather.Hourly.Time.Length && count < 8; i++)
                        {
                            var hour = DateTime.Parse(weather.Hourly.Time[i]);
                            if (hour < now.AddMinutes(-30)) continue;

                            HourlyForecast.Add(new HourlyForecastItem
                            {
                                Time = count == 0 ? "Now" : hour.ToString("h tt"),
                                Temperature = $"{weather.Hourly.Temperature[i]}°",
                                Icon = _weatherService.GetWeatherIcon(weather.Hourly.WeatherCode[i]),
                                IsCurrentHour = count == 0
                            });
                            count++;
                        }
                    }
                }

                IsFavorite = await _databaseService.IsFavoriteAsync(CityName);

                // Fetch AI-generated summary (don't block weather UI on this)
                _ = LoadAiSummaryAsync(weather);
            }
            finally { IsBusy = false; }
        }

        private async Task LoadAiSummaryAsync(WeatherResponse weather)
        {
            if (weather == null) return;
            IsAiSummaryLoading = true;
            AiSummary = null;
            try
            {
                var details = await _weatherService.GetWeatherDetailsAsync(_currentLat, _currentLon, CityName);
                string summary = await _weatherService.GetAiSummaryAsync(
                    CityName,
                    _weatherService.GetWeatherCondition(weather.CurrentWeather.WeatherCode),
                    weather.CurrentWeather.Temperature,
                    details?.Humidity ?? 0,
                    weather.CurrentWeather.WindSpeed);

                AiSummary = summary ?? "Couldn't generate an AI summary right now.";
            }
            finally
            {
                IsAiSummaryLoading = false;
            }
        }

        private async Task ToggleFavoriteAsync()
        {
            if (IsFavorite)
            {
                // Remove from favorites
                var favorites = await _databaseService.GetFavoritesAsync();
                var existing = favorites.FirstOrDefault(f => f.CityName == CityName);
                if (existing != null)
                    await _databaseService.DeleteFavoriteAsync(existing);
                IsFavorite = false;
            }
            else
            {
                // Add to favorites
                string label = await Application.Current.MainPage.DisplayPromptAsync(
                    "Add to Favorites",
                    "Enter a label for this city:",
                    initialValue: CityName,
                    placeholder: "e.g. Home, School, Office");

                if (string.IsNullOrWhiteSpace(label)) return;

                await _databaseService.AddFavoriteAsync(new FavoriteCity
                {
                    CityName = CityName,
                    Latitude = _currentLat,
                    Longitude = _currentLon,
                    Label = label,
                    DateAdded = DateTime.Now
                });
                IsFavorite = true;
            }
        }

        private async Task LoadLastCityOrDefaultAsync()
        {
            var lastCity = Preferences.Get("last_city", "");
            var lastLat = Preferences.Get("last_lat", 40.7128);
            var lastLon = Preferences.Get("last_lon", -74.0060);
            var lastCityName = Preferences.Get("last_city_name", "New York");

            _currentLat = lastLat;
            _currentLon = lastLon;
            CityName = lastCityName;

            await LoadWeatherDataAsync(_currentLat, _currentLon);
        }
        private async Task ViewDetailsAsync()
        {
            await Shell.Current.GoToAsync($"DetailsPage?lat={_currentLat}&lon={_currentLon}&city={Uri.EscapeDataString(CityName)}");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}