using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<FavoriteCity> Favorites { get; } = new();

        public FavoritesViewModel()
        {
            _databaseService = new DatabaseService();
            LoadFavoritesCommand = new Command(async () => await LoadFavoritesAsync());
            DeleteFavoriteCommand = new Command<FavoriteCity>(async (city) => await DeleteFavoriteAsync(city));
            EditFavoriteCommand = new Command<FavoriteCity>(async (city) => await EditFavoriteAsync(city));
            SelectCityCommand = new Command<FavoriteCity>(async (city) => await SelectCityAsync(city));
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public Command LoadFavoritesCommand { get; }
        public Command<FavoriteCity> DeleteFavoriteCommand { get; }
        public Command<FavoriteCity> EditFavoriteCommand { get; }
        public Command<FavoriteCity> SelectCityCommand { get; }

        public async Task LoadFavoritesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var favorites = await _databaseService.GetFavoritesAsync();
                Favorites.Clear();
                foreach (var fav in favorites)
                    Favorites.Add(fav);
            }
            finally { IsBusy = false; }
        }

        public async Task DeleteFavoriteAsync(FavoriteCity city)
        {
            if (city == null) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Remove Favorite",
                $"Remove {city.CityName} from favorites?",
                "Yes", "No");
            if (!confirm) return;

            await _databaseService.DeleteFavoriteAsync(city);
            Favorites.Remove(city);
        }

        public async Task EditFavoriteAsync(FavoriteCity city)
        {
            if (city == null) return;
            string newLabel = await Application.Current.MainPage.DisplayPromptAsync(
                "Edit Label",
                "Enter a new label for this city:",
                initialValue: city.Label ?? city.CityName,
                placeholder: "e.g. Home, School, Office");

            if (string.IsNullOrWhiteSpace(newLabel)) return;

            city.Label = newLabel;
            await _databaseService.UpdateFavoriteAsync(city);
            await LoadFavoritesAsync();
        }

        public async Task SelectCityAsync(FavoriteCity city)
        {
            if (city == null) return;
            await Shell.Current.GoToAsync($"//MainPage?lat={city.Latitude}&lon={city.Longitude}&city={Uri.EscapeDataString(city.CityName)}");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}