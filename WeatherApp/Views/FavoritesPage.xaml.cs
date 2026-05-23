using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
        BindingContext = new FavoritesViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FavoritesViewModel vm)
            await vm.LoadFavoritesAsync();
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is FavoriteCity city)
            if (BindingContext is FavoritesViewModel vm)
                await vm.EditFavoriteAsync(city);
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is FavoriteCity city)
            if (BindingContext is FavoritesViewModel vm)
                await vm.DeleteFavoriteAsync(city);
    }

    private async void OnCityTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is FavoriteCity city)
            if (BindingContext is FavoritesViewModel vm)
                await vm.SelectCityAsync(city);
    }
}