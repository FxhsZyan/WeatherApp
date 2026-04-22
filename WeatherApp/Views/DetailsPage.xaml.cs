using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;

    public DetailsPage(double latitude, double longitude, string cityName)
    {
        InitializeComponent();
        _viewModel = new DetailsViewModel();
        BindingContext = _viewModel;
        Task.Run(async () => await _viewModel.LoadDetailsAsync(latitude, longitude, cityName));
    }
}