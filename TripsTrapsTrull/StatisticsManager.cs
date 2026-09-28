using Microsoft.Maui.Storage;

namespace TripsTrapsTrull
{
    public static class StatisticsManager
    {
        public static int GamesPlayed
        {
            get
            {
                return Preferences.Default.Get(
                    "GamesPlayed",
                    0);
            }
        }


        public static int XWins
        {
            get
            {
                return Preferences.Default.Get(
                    "XWins",
                    0);
            }
        }


        public static int OWins
        {
            get
            {
                return Preferences.Default.Get(
                    "OWins",
                    0);
            }
        }


        public static int Draws
        {
            get
            {
                return Preferences.Default.Get(
                    "Draws",
                    0);
            }
        }


        public static void AddWin(string player)
        {
            int games = GamesPlayed + 1;

            Preferences.Default.Set(
                "GamesPlayed",
                games);


            if (player == "X")
            {
                int wins = XWins + 1;

                Preferences.Default.Set(
                    "XWins",
                    wins);
            }
            else
            {
                int wins = OWins + 1;

                Preferences.Default.Set(
                    "OWins",
                    wins);
            }
        }


        public static void AddDraw()
        {
            int games = GamesPlayed + 1;

            Preferences.Default.Set(
                "GamesPlayed",
                games);


            int draws = Draws + 1;

            Preferences.Default.Set(
                "Draws",
                draws);
        }


        public static void Reset()
        {
            Preferences.Default.Set(
                "GamesPlayed",
                0);

            Preferences.Default.Set(
                "XWins",
                0);

            Preferences.Default.Set(
                "OWins",
                0);

            Preferences.Default.Set(
                "Draws",
                0);
        }
    }
}