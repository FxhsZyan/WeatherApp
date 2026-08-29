# WeatherApp

A mobile weather application built with .NET MAUI, developed as a final project for a Mobile Development course.

## Authors

- Hansen
- Cris Joseph
- Zyann

---

## About

WeatherApp is a cross-platform mobile application that provides real-time weather information for any city in the world. Built using .NET MAUI and C#, the app fetches live data from the Open-Meteo API and displays current conditions, hourly forecasts, a 7-day forecast, and detailed weather metrics. It also uses the Gemini API to generate a short AI-written summary of the day's weather.

This project was built as a final requirement for a Mobile Development course, with the goal of applying concepts such as MVVM architecture, API integration, SQLite local storage, data binding, and multi-page navigation in a real-world mobile application.

---

## Features

- **City Search** — Search any city in the world by name
- **Current Weather** — Displays real-time temperature and weather condition
- **Hourly Forecast** — Shows the next 8 hours of weather from the current time
- **7-Day Forecast** — Shows daily high and low temperatures for the week ahead
- **Weather Details Page** — View wind speed, humidity, visibility, and UV index
- **AI Weather Summary** — Generates a short, friendly summary and practical tip using the Gemini API
- **Favorites** — Save cities with a custom label and load them with one tap
- **Search History** — Automatically tracks the last 20 searched cities
- **Weather Icons** — Custom icons for different weather conditions
- **Clean UI** — Frosted glass-style cards with a sky gradient background
- **Persistent Storage** — Last searched city is remembered between app sessions

---

## Built With

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) — Cross-platform UI framework (net10.0)
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) — Primary programming language
- [Open-Meteo API](https://open-meteo.com/) — Free weather forecast API (no key required)
- [Open-Meteo Geocoding API](https://open-meteo.com/en/docs/geocoding-api) — City name to coordinates lookup
- [Gemini API](https://ai.google.dev/) — AI-generated weather summaries (requires a free API key)
- [SQLite-net-pcl](https://github.com/praeclarum/sqlite-net) — Local database for favorites and history
- Visual Studio 2022

---

## Project Structure

```
WeatherApp/
├── Converters/
│   └── NullToBoolConverter.cs
├── Models/
│   ├── FavoriteCity.cs
│   ├── SearchHistory.cs
│   ├── WeatherData.cs
│   └── WeatherDetails.cs
├── Services/
│   ├── WeatherService.cs
│   ├── DatabaseService.cs
│   └── Secrets.cs           # Not tracked in git — see Setup below
├── ViewModels/
│   ├── MainViewModel.cs
│   ├── DetailsViewModel.cs
│   ├── FavoritesViewModel.cs
│   ├── HistoryViewModel.cs
│   └── OnboardingViewModel.cs
├── Views/
│   ├── DetailsPage.xaml
│   ├── FavoritesPage.xaml
│   ├── HistoryPage.xaml
│   ├── HelpPage.xaml
│   └── OnboardingPage.xaml
├── Resources/
│   ├── AppIcon/
│   │   ├── appicon.png
│   │   └── appiconfg.svg
│   ├── Images/
│   │   ├── weather_bg_day.png
│   │   ├── weather_icon_sunny.png
│   │   ├── weather_icon_cloudy.png
│   │   ├── weather_icon_rainy.png
│   │   ├── weather_icon_wind.png
│   │   ├── weather_icon_humidity.png
│   │   ├── weather_icon_visibility.png
│   │   └── weather_icon_uv.png
│   ├── Fonts/
│   │   ├── OpenSans-Regular.ttf
│   │   └── OpenSans-Semibold.ttf
│   └── Splash/
│       └── splash.svg
├── MainPage.xaml
├── AppShell.xaml
├── App.xaml
├── MauiProgram.cs
└── WeatherApp.csproj
```

---

## Getting Started

### Prerequisites

- Visual Studio 2022 with the **.NET MAUI** workload installed
- .NET 10.0 SDK
- Android SDK (for Android deployment) or Windows 10/11
- A free Gemini API key (see Setup below) — required for the AI weather summary feature

### Setup: Gemini API Key (required)

The AI weather summary feature calls the Gemini API using a key that is **not included in this repository** for security reasons (`Secrets.cs` is gitignored so no one's personal key gets committed). Each teammate needs to create their own local copy:

1. Get a free API key from [Google AI Studio](https://aistudio.google.com/apikey)
2. In Visual Studio, right-click the `Services` folder → **Add → Class...** → name it `Secrets.cs`
3. Paste the following, replacing the placeholder with your actual key:

   ```csharp
   namespace WeatherApp.Services
   {
       public static class Secrets
       {
           public const string GeminiApiKey = "YOUR_API_KEY_HERE";
       }
   }
   ```

4. Save the file. Do **not** commit this file — it should already be listed in `.gitignore`.

Without this file, the project will fail to build with error `CS0103: The name 'Secrets' does not exist in the current context`.

### Running the App

1. Clone this repository
   ```
   git clone https://github.com/your-username/WeatherApp.git
   ```
2. Complete the Gemini API key setup above
3. Open `WeatherApp.slnx` in Visual Studio 2022
4. Set the run target to **Android Emulator**, **Android Device**, or **Windows Machine**
5. Press **F5** to build and run

> Weather data itself uses the free [Open-Meteo API](https://open-meteo.com/) and needs no key. Only the AI summary feature requires the Gemini API key above.

---

## API Reference

| API | Purpose | Key Required |
|---|---|---|
| `api.open-meteo.com/v1/forecast` | Current weather, hourly forecast, and 7-day forecast | No |
| `geocoding-api.open-meteo.com/v1/search` | City name to coordinates lookup | No |
| `generativelanguage.googleapis.com` (Gemini) | AI-generated weather summary | Yes — see Setup above |

---

## License

This project was made for educational purposes as part of a Mobile Development course final project.


## Setup
This app uses the Gemini API for AI-generated weather summaries.
1. Copy `Services/Secrets.example.cs` and rename it to `Services/Secrets.cs`
2. Get a free API key from https://aistudio.google.com
3. Paste your key into the `GeminiApiKey` value in `Secrets.cs`