using SQLite;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace TripsTrapsTrull
{
    //Andmebaasi struktuur
    public class StatItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int GamesPlayed { get; set; }
        public int XWins { get; set; }
        public int OWins { get; set; }
        public int Draws { get; set; }
    }

    public static class DatabaseService
    {
        static SQLiteAsyncConnection _database;

        static async Task Init()
        {
            if (_database != null)
                return;

            // Andmebaasifaili asukoht seadmes/arvutis
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "trull_database.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // Loome tabeli, kui seda veel pole
            await _database.CreateTableAsync<StatItem>();
        }

        public static async Task<StatItem> GetStatsAsync()
        {
            await Init();
            var stats = await _database.Table<StatItem>().FirstOrDefaultAsync();

            // Kui andmebaas on tühi, loome esimese rea
            if (stats == null)
            {
                stats = new StatItem { GamesPlayed = 0, XWins = 0, OWins = 0, Draws = 0 };
                await _database.InsertAsync(stats);
            }
            return stats;
        }

        public static async Task SaveStatsAsync(StatItem stats)
        {
            await Init();
            await _database.UpdateAsync(stats);
        }

        public static async Task ResetStatsAsync()
        {
            await Init();
            var stats = await GetStatsAsync();
            stats.GamesPlayed = 0;
            stats.XWins = 0;
            stats.OWins = 0;
            stats.Draws = 0;
            await _database.UpdateAsync(stats);
        }
    }
}