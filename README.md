# WeatherApp 🌤️

A mobile weather application built with .NET MAUI for Windows, developed as a final project for a Mobile Development course.

## 👥 Authors

- Hansen
- Cris Joseph
- Zyann

---

## 📖 About

WeatherApp is a cross-platform mobile application that provides real-time weather information for any city in the world. Built using .NET MAUI and C#, the app fetches live data from the Open-Meteo API and displays current conditions, a 7-day forecast, and detailed weather metrics.

This project was built as our final requirement for our Mobile Development course, with the goal of applying concepts such as MVVM architecture, API integration, data binding, and multi-page navigation in a real-world mobile application.

---

## ✨ Features

- 🔍 **City Search** — Search any city in the world by name
- 🌡️ **Current Weather** — Displays real-time temperature and weather condition
- 📅 **7-Day Forecast** — Shows daily high and low temperatures for the week ahead
- 💨 **Weather Details Page** — View wind speed, humidity, visibility, and UV index
- 🖼️ **Weather Icons** — Custom icons for different weather conditions
- 📱 **Clean UI** — Frosted glass-style cards with a sky gradient background

---

## 🛠️ Built With

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) — Cross-platform UI framework
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) — Primary programming language
- [Open-Meteo API](https://open-meteo.com/) — Free weather forecast API
- [Open-Meteo Geocoding API](https://open-meteo.com/en/docs/geocoding-api) — City name to coordinates lookup
- Visual Studio 2022

---

## 🏗️ Project Structure

```
WeatherApp/
├── Converters/
│   └── NullToBoolConverter.cs
├── Models/
│   ├── WeatherData.cs
│   └── WeatherDetails.cs
├── Services/
│   └── WeatherService.cs
├── ViewModels/
│   ├── MainViewModel.cs
│   └── DetailsViewModel.cs
├── Views/
│   └── DetailsPage.xaml
├── Resources/
│   └── Images/
│       ├── weather_bg_day.png
│       ├── weather_icon_sunny.png
│       ├── weather_icon_cloudy.png
│       ├── weather_icon_rainy.png
│       ├── weather_icon_wind.png
│       ├── weather_icon_humidity.png
│       ├── weather_icon_visibility.png
│       └── weather_icon_uv.png
├── MainPage.xaml
├── AppShell.xaml
├── App.xaml
└── MauiProgram.cs
```

---

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 with the **.NET MAUI** workload installed
- .NET 8.0 SDK or later
- Windows 10/11

### Running the App

1. Clone this repository
   ```
   git clone https://github.com/your-username/WeatherApp.git
   ```
2. Open `WeatherApp.sln` in Visual Studio 2022
3. Set the run target to **Windows Machine**
4. Press **F5** to build and run

> No API key required — the app uses the free [Open-Meteo API](https://open-meteo.com/).

---

## 📡 API Reference

This app uses the following free, no-auth APIs:

| API | Purpose |
|---|---|
| `api.open-meteo.com/v1/forecast` | Current weather + 7-day forecast |
| `geocoding-api.open-meteo.com/v1/search` | City name to coordinates |

---

## 📝 License

This project was made for educational purposes as part of a Mobile Development course final project.
