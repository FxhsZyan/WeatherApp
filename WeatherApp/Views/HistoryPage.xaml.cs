using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HistoryViewModel vm)
            await vm.LoadHistoryAsync();
    }
}