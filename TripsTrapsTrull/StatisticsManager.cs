namespace TripsTrapsTrull
{
    public static class StatisticsManager
    {
        public static async Task<(int games, int xWins, int oWins, int draws)> GetStatsAsync()
        {
            var stats = await DatabaseService.GetStatsAsync();
            return (stats.GamesPlayed, stats.XWins, stats.OWins, stats.Draws);
        }

        public static async Task AddWinAsync(string player)
        {
            var stats = await DatabaseService.GetStatsAsync();
            stats.GamesPlayed++;

            if (player == "X")
                stats.XWins++;
            else
                stats.OWins++;

            await DatabaseService.SaveStatsAsync(stats);
        }

        public static async Task AddDrawAsync()
        {
            var stats = await DatabaseService.GetStatsAsync();
            stats.GamesPlayed++;
            stats.Draws++;

            await DatabaseService.SaveStatsAsync(stats);
        }

        public static async Task ResetAsync()
        {
            await DatabaseService.ResetStatsAsync();
        }
    }
}