namespace WeatherApp.Views;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
    }

    private async void OnReplayOnboardingClicked(object sender, EventArgs e)
    {
        // Clear the flag so it feels fresh, then push as a modal
        var onboardingPage = new OnboardingPage();
        await Navigation.PushModalAsync(onboardingPage, animated: true);
    }
}
