using SQLite;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task InitAsync()
        {
            if (_database != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "weatherapp.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<FavoriteCity>();
            await _database.CreateTableAsync<SearchHistory>();
        }

        // ── FAVORITES CRUD ──────────────────────────────────────

        public async Task<List<FavoriteCity>> GetFavoritesAsync()
        {
            await InitAsync();
            return await _database.Table<FavoriteCity>()
                .OrderByDescending(f => f.DateAdded)
                .ToListAsync();
        }

        public async Task AddFavoriteAsync(FavoriteCity city)
        {
            await InitAsync();
            await _database.InsertAsync(city);
        }

        public async Task UpdateFavoriteAsync(FavoriteCity city)
        {
            await InitAsync();
            await _database.UpdateAsync(city);
        }

        public async Task DeleteFavoriteAsync(FavoriteCity city)
        {
            await InitAsync();
            await _database.DeleteAsync(city);
        }

        public async Task<bool> IsFavoriteAsync(string cityName)
        {
            await InitAsync();
            var existing = await _database.Table<FavoriteCity>()
                .Where(f => f.CityName == cityName)
                .FirstOrDefaultAsync();
            return existing != null;
        }

        // ── SEARCH HISTORY CRUD ─────────────────────────────────

        public async Task<List<SearchHistory>> GetHistoryAsync()
        {
            await InitAsync();
            return await _database.Table<SearchHistory>()
                .OrderByDescending(h => h.SearchedAt)
                .ToListAsync();
        }

        public async Task AddHistoryAsync(SearchHistory history)
        {
            await InitAsync();

            // avoid duplicates — delete old entry if same city exists
            var existing = await _database.Table<SearchHistory>()
                .Where(h => h.CityName == history.CityName)
                .FirstOrDefaultAsync();
            if (existing != null)
                await _database.DeleteAsync(existing);

            await _database.InsertAsync(history);

            // keep only last 20 searches
            var all = await GetHistoryAsync();
            if (all.Count > 20)
            {
                var toDelete = all.Skip(20).ToList();
                foreach (var item in toDelete)
                    await _database.DeleteAsync(item);
            }
        }

        public async Task DeleteHistoryItemAsync(SearchHistory item)
        {
            await InitAsync();
            await _database.DeleteAsync(item);
        }

        public async Task ClearHistoryAsync()
        {
            await InitAsync();
            await _database.DeleteAllAsync<SearchHistory>();
        }
    }
}