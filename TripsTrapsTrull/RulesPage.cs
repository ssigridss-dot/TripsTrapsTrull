namespace TripsTrapsTrull
{
    public class RulesPage : ContentPage
    {
        private GameSettings settings;
        private Label titleLabel;
        private Label rulesLabel;

        public RulesPage(GameSettings settings)
        {
            this.settings = settings;
            Title = "Reeglid";

            titleLabel = new Label
            {
                Text = "Mängureeglid",
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            rulesLabel = new Label
            {
                Text =
                    "1. Mäng toimub kahe mängija vahel.\n\n" +
                    "2. Mängijad kasutavad erinevaid sümboleid.\n\n" +
                    "3. Mängija vajutab vabale ruudule, et sinna oma sümbol paigutada.\n\n" +
                    "4. 3x3 mänguväljal tuleb võitmiseks saada kolm oma sümbolit järjest.\n\n" +
                    "5. 4x4 mänguväljal tuleb saada neli sümbolit järjest.\n\n" +
                    "6. 5x5 mänguväljal tuleb saada viis sümbolit järjest.\n\n" +
                    "7. Järjestikused sümbolid võivad olla horisontaalselt, vertikaalselt või diagonaalselt.\n\n" +
                    "8. Kui kõik ruudud on täidetud ja keegi ei võida, on tulemuseks viik.\n\n" +
                    "9. Botiga mängides mängib arvuti teise mängijana.\n\n",
                FontSize = 16
            };

            Button backButton = new Button
            {
                Text = "Tagasi",
                FontSize = 15,
                HeightRequest = 45,
                WidthRequest = 150,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            backButton.Clicked += async (sender, e) =>
            {
                await Navigation.PopAsync();
            };

            VerticalStackLayout layout = new VerticalStackLayout
            {
                Padding = 25,
                Spacing = 15,
                Children =
                {
                    titleLabel,
                    rulesLabel,
                    backButton
                }
            };

            Content = new ScrollView { Content = layout };
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            bool isDark = settings.DarkTheme;
            BackgroundColor = isDark ? Color.FromArgb("#121212") : Color.FromArgb("#FAF7F2");
            Color textColor = isDark ? Colors.White : Color.FromArgb("#222222");

            titleLabel.TextColor = textColor;
            rulesLabel.TextColor = textColor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyTheme();
        }
    }
}