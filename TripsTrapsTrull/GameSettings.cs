namespace TripsTrapsTrull
{
    public class GameSettings
    {
        // Mänguvälja suurus
        public int BoardSize { get; set; } = 3;

        // Kas mängitakse botiga
        public bool BotEnabled { get; set; } = false;

        // Kes alustab
        public string Starter { get; set; } = "X";

        // Mängijate sümbolid
        public string XSymbol { get; set; } = "X";
        public string OSymbol { get; set; } = "O";

        // Mängijate värvid
        public string XColor { get; set; } = "#D85B7F";
        public string OColor { get; set; } = "#4D78C2";

        // Rakenduse taust
        public string BackgroundColor { get; set; } = "#FAF7F2";

        // Tume või hele teema
        public bool DarkTheme { get; set; } = false;
    }
}