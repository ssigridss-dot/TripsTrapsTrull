using Microsoft.Maui.Controls;

namespace TripsTrapsTrull
{
    public class SettingsPage : ContentPage
    {
        private GameSettings settings;


        public SettingsPage(GameSettings settings)
        {
            this.settings = settings;

            Title = "Seaded";


            Label title = new Label
            {
                Text = "⚙ Seaded",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };


            Button themeButton = new Button
            {
                Text = "Vali hele/tume teema"
            };


            themeButton.Clicked += async (sender, e) =>
            {
                string result =
                    await DisplayActionSheetAsync(
                        "Vali teema",
                        "Tühista",
                        null,
                        "Hele",
                        "Tume");


                if (result == "Hele")
                {
                    settings.DarkTheme = false;

                    Application.Current!.UserAppTheme =
                        AppTheme.Light;
                }
                else if (result == "Tume")
                {
                    settings.DarkTheme = true;

                    Application.Current!.UserAppTheme =
                        AppTheme.Dark;
                }
            };


            Button backgroundButton = new Button
            {
                Text = "Vali taust"
            };


            backgroundButton.Clicked += async (sender, e) =>
            {
                string result =
                    await DisplayActionSheetAsync(
                        "Vali taust",
                        "Tühista",
                        null,
                        "Hele",
                        "Heleroosa",
                        "Heleroheline",
                        "Helesinine");


                if (result == "Hele")
                {
                    settings.BackgroundColor = "#FAF7F2";
                }
                else if (result == "Heleroosa")
                {
                    settings.BackgroundColor = "#FFF0F5";
                }
                else if (result == "Heleroheline")
                {
                    settings.BackgroundColor = "#F0FFF4";
                }
                else if (result == "Helesinine")
                {
                    settings.BackgroundColor = "#F0F7FF";
                }

                BackgroundColor =
                    Color.FromArgb(
                        settings.BackgroundColor);
            };


            Button backButton = new Button
            {
                Text = "Salvesta ja tagasi"
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
                        themeButton,
                        backgroundButton,
                        backButton
                    }
                };


            Content = layout;

            BackgroundColor =
                Color.FromArgb(
                    settings.BackgroundColor);
        }
    }
}