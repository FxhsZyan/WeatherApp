using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SearchHistory> History { get; } = new();

        public HistoryViewModel()
        {
            _databaseService = new DatabaseService();
            LoadHistoryCommand = new Command(async () => await LoadHistoryAsync());
            DeleteHistoryItemCommand = new Command<SearchHistory>(async (item) => await DeleteHistoryItemAsync(item));
            ClearHistoryCommand = new Command(async () => await ClearHistoryAsync());
            SelectHistoryItemCommand = new Command<SearchHistory>(async (item) => await SelectHistoryItemAsync(item));
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand LoadHistoryCommand { get; }
        public ICommand DeleteHistoryItemCommand { get; }
        public ICommand ClearHistoryCommand { get; }
        public ICommand SelectHistoryItemCommand { get; }

        public async Task LoadHistoryAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var history = await _databaseService.GetHistoryAsync();
                History.Clear();
                foreach (var item in history)
                    History.Add(item);
            }
            finally { IsBusy = false; }
        }

        private async Task DeleteHistoryItemAsync(SearchHistory item)
        {
            if (item == null) return;
            await _databaseService.DeleteHistoryItemAsync(item);
            History.Remove(item);
        }

        private async Task ClearHistoryAsync()
        {
            if (History.Count == 0) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Clear History",
                "Are you sure you want to clear all search history?",
                "Yes", "No");
            if (!confirm) return;

            await _databaseService.ClearHistoryAsync();
            History.Clear();
        }

        private async Task SelectHistoryItemAsync(SearchHistory item)
        {
            if (item == null) return;
            await Shell.Current.GoToAsync($"//MainPage?lat={item.Latitude}&lon={item.Longitude}&city={Uri.EscapeDataString(item.CityName)}");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}