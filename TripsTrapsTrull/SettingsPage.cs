namespace TripsTrapsTrull
{
    public class SettingsPage : ContentPage
    {
        private GameSettings settings;
        private Label titleLabel;
        private Button themeButton;

        public SettingsPage(GameSettings settings)
        {
            this.settings = settings;
            Title = "Seaded";

            titleLabel = new Label
            {
                Text = "Seaded",
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            themeButton = new Button
            {
                Text = settings.DarkTheme ? "Teema: Tume" : "Teema: Hele",
                FontSize = 15,
                HeightRequest = 45,
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center
            };

            themeButton.Clicked += async (sender, e) =>
            {
                string result = await DisplayActionSheetAsync("Vali teema", "Tühista", null, "Hele", "Tume");

                if (result == "Hele")
                {
                    settings.DarkTheme = false;
                    Application.Current!.UserAppTheme = AppTheme.Light;

                    // Värskendame navigeerimisriba koheselt heledaks
                    if (Application.Current?.MainPage is NavigationPage navPage)
                    {
                        navPage.BarBackgroundColor = Color.FromArgb("#FAF7F2");
                        navPage.BarTextColor = Color.FromArgb("#222222");
                    }
                }
                else if (result == "Tume") // või "Tume" (jälgi oma koodi stringi)
                {
                    settings.DarkTheme = true;
                    Application.Current!.UserAppTheme = AppTheme.Dark;

                    // Värskendame navigeerimisriba koheselt tumedaks
                    if (Application.Current?.MainPage is NavigationPage navPage)
                    {
                        navPage.BarBackgroundColor = Color.FromArgb("#121212");
                        navPage.BarTextColor = Colors.White;
                    }
                }

                themeButton.Text = settings.DarkTheme ? "Teema: Tume" : "Teema: Hele";
                ApplyThemeColors();
            };

            Button backButton = new Button
            {
                Text = "Tagasi",
                FontSize = 15,
                HeightRequest = 45,
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center
            };

            backButton.Clicked += async (sender, e) =>
            {
                await Navigation.PopAsync();
            };

            VerticalStackLayout layout = new VerticalStackLayout
            {
                Padding = 25,
                Spacing = 20,
                Children =
                {
                    titleLabel,
                    themeButton,
                    backButton
                }
            };

            Content = layout;
            ApplyThemeColors();
        }

        private void ApplyThemeColors()
        {
            bool isDark = settings.DarkTheme;

            // Globaalne teema muutus rakenduse tasemel
            Application.Current!.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;

            // Lehe taust ja pealkirja värv
            BackgroundColor = isDark ? Color.FromArgb("#121212") : Color.FromArgb("#FAF7F2");
            titleLabel.TextColor = isDark ? Colors.White : Color.FromArgb("#222222");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyThemeColors();
        }
    }
}