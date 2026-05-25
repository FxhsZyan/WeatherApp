using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class OnboardingPage : ContentPage
{
    private readonly OnboardingViewModel _viewModel;

    public OnboardingPage()
    {
        InitializeComponent();
        _viewModel = new OnboardingViewModel();
        BindingContext = _viewModel;
    }

    private void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        int index = _viewModel.Slides.IndexOf(_viewModel.Slides.FirstOrDefault(s => s == e.CurrentItem));
        bool isLast = index == _viewModel.Slides.Count - 1;

        NextButton.Text = isLast ? "Get Started 🌤️" : "Next →";
        SkipButton.IsVisible = !isLast;
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        int current = OnboardingCarousel.Position;
        if (current < _viewModel.Slides.Count - 1)
        {
            OnboardingCarousel.Position = current + 1;
        }
        else
        {
            FinishOnboarding();
        }
    }

    private void OnSkipClicked(object sender, EventArgs e)
    {
        FinishOnboarding();
    }

    private void FinishOnboarding()
    {
        // Mark onboarding as completed so it never shows again
        Preferences.Set("HasCompletedOnboarding", true);

        // Use CreateWindow pattern — update the window's root page directly
        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }
}
