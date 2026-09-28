using Microsoft.Maui.Controls;

namespace TripsTrapsTrull
{
    public class StatisticsPage : ContentPage
    {
        Label gamesLabel;
        Label xWinsLabel;
        Label oWinsLabel;
        Label drawsLabel;


        public StatisticsPage()
        {
            Title = "Statistika";


            Label title = new Label
            {
                Text = "Mängustatistika",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };


            gamesLabel = new Label
            {
                FontSize = 20
            };


            xWinsLabel = new Label
            {
                FontSize = 20
            };


            oWinsLabel = new Label
            {
                FontSize = 20
            };


            drawsLabel = new Label
            {
                FontSize = 20
            };


            Button resetButton = new Button
            {
                Text = "Kustuta statistika"
            };


            resetButton.Clicked += async (sender, e) =>
            {
                bool answer = await DisplayAlertAsync(
                    "Kustuta statistika",
                    "Kas soovid kõik tulemused kustutada?",
                    "Jah",
                    "Ei");


                if (answer)
                {
                    StatisticsManager.Reset();

                    UpdateStatistics();

                    await DisplayAlertAsync(
                        "Statistika",
                        "Statistika on kustutatud.",
                        "OK");
                }
            };


            Button backButton = new Button
            {
                Text = "Tagasi"
            };


            backButton.Clicked += async (sender, e) =>
            {
                await Navigation.PopAsync();
            };


            VerticalStackLayout layout =
                new VerticalStackLayout
                {
                    Padding = 25,
                    Spacing = 20,

                    Children =
                    {
                        title,
                        gamesLabel,
                        xWinsLabel,
                        oWinsLabel,
                        drawsLabel,
                        resetButton,
                        backButton
                    }
                };


            Content = layout;


            UpdateStatistics();
        }


        private void UpdateStatistics()
        {
            gamesLabel.Text =
                "Mänge kokku: " +
                StatisticsManager.GamesPlayed;


            xWinsLabel.Text =
                "X võidud: " +
                StatisticsManager.XWins;


            oWinsLabel.Text =
                "O võidud: " +
                StatisticsManager.OWins;


            drawsLabel.Text =
                "Viigid: " +
                StatisticsManager.Draws;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateStatistics();
        }
    }
}