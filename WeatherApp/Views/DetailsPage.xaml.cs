using WeatherApp.ViewModels;

namespace WeatherApp.Views;

[QueryProperty(nameof(Lat), "lat")]
[QueryProperty(nameof(Lon), "lon")]
[QueryProperty(nameof(City), "city")]
public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;

    public string Lat { set => TryLoad(double.Parse(value), _lon, _city); }
    public string Lon { set { _lon = double.Parse(value); } }
    public string City { set { _city = Uri.UnescapeDataString(value); } }

    private double _lat, _lon;
    private string _city;

    public DetailsPage()
    {
        InitializeComponent();
        _viewModel = new DetailsViewModel();
        BindingContext = _viewModel;
    }

    private void TryLoad(double lat, double lon, string city)
    {
        _lat = lat;
        Task.Run(async () => await _viewModel.LoadDetailsAsync(_lat, _lon, _city));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_lat != 0 && _lon != 0)
            await _viewModel.LoadDetailsAsync(_lat, _lon, _city);
    }
}