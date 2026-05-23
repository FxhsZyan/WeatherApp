using SQLite;

namespace WeatherApp.Models
{
    [Table("SearchHistory")]
    public class SearchHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string CityName { get; set; }

        [NotNull]
        public double Latitude { get; set; }

        [NotNull]
        public double Longitude { get; set; }

        public DateTime SearchedAt { get; set; } = DateTime.Now;
    }
}