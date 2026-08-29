# WeatherApp 🌤️

A mobile weather application built with .NET MAUI, developed as a final project for a Mobile Development course.

## 👥 Authors

- Hansen
- Cris Joseph
- Zyann

---

## 📖 About

WeatherApp is a cross-platform mobile application that provides real-time weather information for any city in the world. Built using .NET MAUI and C#, the app fetches live data from the Open-Meteo API and displays current conditions, hourly forecasts, a 7-day forecast, and detailed weather metrics.

This project was built as a final requirement for a Mobile Development course, with the goal of applying concepts such as MVVM architecture, API integration, SQLite local storage, data binding, and multi-page navigation in a real-world mobile application.

---

## ✨ Features

- 🔍 **City Search** — Search any city in the world by name
- 🌡️ **Current Weather** — Displays real-time temperature and weather condition
- ⏱️ **Hourly Forecast** — Shows the next 8 hours of weather from the current time
- 📅 **7-Day Forecast** — Shows daily high and low temperatures for the week ahead
- 💨 **Weather Details Page** — View wind speed, humidity, visibility, and UV index
- ⭐ **Favorites** — Save cities with a custom label and load them with one tap
- 🕓 **Search History** — Automatically tracks the last 20 searched cities
- 🖼️ **Weather Icons** — Custom icons for different weather conditions
- 📱 **Clean UI** — Frosted glass-style cards with a sky gradient background
- 💾 **Persistent Storage** — Last searched city is remembered between app sessions

---

## 🛠️ Built With

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) — Cross-platform UI framework (net10.0)
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) — Primary programming language
- [Open-Meteo API](https://open-meteo.com/) — Free weather forecast API (no key required)
- [Open-Meteo Geocoding API](https://open-meteo.com/en/docs/geocoding-api) — City name to coordinates lookup
- [SQLite-net-pcl](https://github.com/praeclarum/sqlite-net) — Local database for favorites and history
- Visual Studio 2022

---

## 🏗️ Project Structure

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
│   └── DatabaseService.cs
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

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 with the **.NET MAUI** workload installed
- .NET 10.0 SDK
- Android SDK (for Android deployment) or Windows 10/11

### Running the App

1. Clone this repository
   ```
   git clone https://github.com/your-username/WeatherApp.git
   ```
2. Open `WeatherApp.slnx` in Visual Studio 2022
3. Set the run target to **Android Emulator**, **Android Device**, or **Windows Machine**
4. Press **F5** to build and run

> No API key required — the app uses the free [Open-Meteo API](https://open-meteo.com/).

---

## 📡 API Reference

| API | Purpose |
|---|---|
| `api.open-meteo.com/v1/forecast` | Current weather, hourly forecast, and 7-day forecast |
| `geocoding-api.open-meteo.com/v1/search` | City name to coordinates lookup |

---

## 📝 License

This project was made for educational purposes as part of a Mobile Development course final project.


## Setup
This app uses the Gemini API for AI-generated weather summaries.
1. Copy `Services/Secrets.example.cs` and rename it to `Services/Secrets.cs`
2. Get a free API key from https://aistudio.google.com
3. Paste your key into the `GeminiApiKey` value in `Secrets.cs`