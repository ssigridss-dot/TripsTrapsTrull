using Microsoft.Maui.Controls;

namespace TripsTrapsTrull
{
    public class RulesPage : ContentPage
    {
        public RulesPage()
        {
            Title = "Mängureeglid";


            Label title = new Label
            {
                Text = "Trips-Traps-Trull",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };


            Label rules = new Label
            {
                Text =
                    "Mängureeglid:\n\n" +

                    "1. Mäng toimub kahe mängija vahel.\n\n" +

                    "2. Mängijad kasutavad erinevaid sümboleid.\n\n" +

                    "3. Mängija vajutab vabale ruudule, " +
                    "et sinna oma sümbol paigutada.\n\n" +

                    "4. 3x3 mänguväljal tuleb võitmiseks saada " +
                    "kolm oma sümbolit järjest.\n\n" +

                    "5. 4x4 mänguväljal tuleb saada neli " +
                    "sümbolit järjest.\n\n" +

                    "6. 5x5 mänguväljal tuleb saada viis " +
                    "sümbolit järjest.\n\n" +

                    "7. Järjestikused sümbolid võivad olla " +
                    "horisontaalselt, vertikaalselt või diagonaalselt.\n\n" +

                    "8. Kui kõik ruudud on täidetud ja keegi " +
                    "ei võida, on tulemuseks viik.\n\n" +

                    "9. Botiga mängides mängib arvuti " +
                    "teise mängijana.\n\n",

                FontSize = 18
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
                        rules,
                        backButton
                    }
                };


            Content = new ScrollView
            {
                Content = layout
            };
        }
    }
}