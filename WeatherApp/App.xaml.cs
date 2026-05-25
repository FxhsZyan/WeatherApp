using WeatherApp.Views;

namespace WeatherApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Show onboarding only on the very first launch.
        // Preferences.Get returns false when the key has never been set.
        bool hasOnboarded = Preferences.Get("HasCompletedOnboarding", false);

        Page rootPage = hasOnboarded
            ? new AppShell()
            : new NavigationPage(new OnboardingPage());

        return new Window(rootPage);
    }
}
