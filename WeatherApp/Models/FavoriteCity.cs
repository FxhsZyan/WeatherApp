using SQLite;

namespace WeatherApp.Models
{
    [Table("FavoriteCities")]
    public class FavoriteCity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string CityName { get; set; }

        [NotNull]
        public double Latitude { get; set; }

        [NotNull]
        public double Longitude { get; set; }

        public string Label { get; set; } // e.g. "Home", "School", or just the city name

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}