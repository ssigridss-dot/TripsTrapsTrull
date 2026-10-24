namespace TripsTrapsTrull
{
    public class StatisticsPage : ContentPage
    {
        Label titleLabel;
        Label gamesLabel;
        Label xWinsLabel;
        Label oWinsLabel;
        Label drawsLabel;
        GameSettings settings;

        public StatisticsPage(GameSettings settings)
        {
            this.settings = settings;
            Title = "Statistika";

            titleLabel = new Label
            {
                Text = "Mängustatistika",
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            gamesLabel = new Label { FontSize = 18 };
            xWinsLabel = new Label { FontSize = 18 };
            oWinsLabel = new Label { FontSize = 18 };
            drawsLabel = new Label { FontSize = 18 };

            Button resetButton = new Button
            {
                Text = "Kustuta",
                FontSize = 14,
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.Fill
            };

            resetButton.Clicked += async (sender, e) =>
            {
                bool answer = await DisplayAlertAsync("Kustuta statistika", "Kas soovid kõik tulemused kustutada?", "Jah", "Ei");
                if (answer)
                {
                    await StatisticsManager.ResetAsync();
                    UpdateStatistics();
                    await DisplayAlertAsync("Statistika", "Statistika on kustutatud.", "OK");
                }
            };

            Button backButton = new Button
            {
                Text = "Tagasi",
                FontSize = 14,
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.Fill
            };

            backButton.Clicked += async (sender, e) =>
            {
                await Navigation.PopAsync();
            };

            Grid buttonsGrid = new Grid
            {
                ColumnSpacing = 10,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 300
            };
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            buttonsGrid.Add(resetButton, 0, 0);
            buttonsGrid.Add(backButton, 1, 0);

            VerticalStackLayout layout = new VerticalStackLayout
            {
                Padding = 25,
                Spacing = 15,
                Children =
                {
                    titleLabel,
                    gamesLabel,
                    xWinsLabel,
                    oWinsLabel,
                    drawsLabel,
                    buttonsGrid
                }
            };

            Content = new ScrollView { Content = layout };
            ApplyTheme();
            UpdateStatistics();
        }

        private void ApplyTheme()
        {
            bool isDark = settings.DarkTheme;
            BackgroundColor = isDark ? Color.FromArgb("#121212") : Color.FromArgb("#FAF7F2");
            Color textColor = isDark ? Colors.White : Color.FromArgb("#222222");

            titleLabel.TextColor = textColor;
            gamesLabel.TextColor = textColor;
            xWinsLabel.TextColor = textColor;
            oWinsLabel.TextColor = textColor;
            drawsLabel.TextColor = textColor;
        }

        private async void UpdateStatistics()
        {
            var stats = await StatisticsManager.GetStatsAsync();
            gamesLabel.Text = "Mänge kokku: " + stats.games;
            xWinsLabel.Text = "X võidud: " + stats.xWins;
            oWinsLabel.Text = "O võidud: " + stats.oWins;
            drawsLabel.Text = "Viigid: " + stats.draws;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyTheme();
            UpdateStatistics();
        }
    }
}