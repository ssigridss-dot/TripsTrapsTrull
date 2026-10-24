namespace TripsTrapsTrull
{
    public class TrullPage : ContentPage
    {
        private Grid gameGrid;
        private Grid boardContainer;
        private Button[,] gameButtons;
        private Button botButton;
        private GraphicsView winningLineView;
        private WinningLineDrawable winningLineDrawable;
        private Label playerLabel;
        private Label sizeLabel;
        private GameSettings settings;
        private GameLogic game;
        private Random rnd = new Random();
        private bool gameOver = false;

        public TrullPage()
        {
            settings = new GameSettings();
            game = new GameLogic(settings.BoardSize);
            gameButtons = new Button[settings.BoardSize, settings.BoardSize];

            BuildPage();
            StartNewGame();
        }

        private void BuildPage()
        {
            Title = "Trips-Traps-Trull";

            playerLabel = new Label
            {
                Text = "Mängija X kord",
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            sizeLabel = new Label
            {
                Text = "Mänguväli: 3 × 3",
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Center
            };

            gameGrid = Tee_grid3X3();

            winningLineDrawable = new WinningLineDrawable();

            winningLineView = new GraphicsView
            {
                Drawable = winningLineDrawable,
                WidthRequest = 330,
                HeightRequest = 330,
                InputTransparent = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            buttonsGridSetup();

            boardContainer = new Grid
            {
                WidthRequest = 340,
                HeightRequest = 340,
                Padding = 5,
                BackgroundColor = Colors.DarkGray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            boardContainer.Add(gameGrid);
            boardContainer.Add(winningLineView);

            VerticalStackLayout layout = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 15,
                Children =
                {
                    playerLabel,
                    sizeLabel,
                    boardContainer,
                    buttonsGrid
                }
            };

            ScrollView scrollView = new ScrollView
            {
                Content = layout
            };

            Content = scrollView;

            ApplySettings();
        }

        private Grid buttonsGrid;

        private void buttonsGridSetup()
        {
            buttonsGrid = new Grid
            {
                ColumnSpacing = 10,
                RowSpacing = 10,
                HorizontalOptions = LayoutOptions.Center
            };

            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            for (int i = 0; i < 5; i++)
            {
                buttonsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            AddButtonToGrid("Uus mäng", 0, 0, (s, e) => StartNewGame());
            AddButtonToGrid("Kes alustab?", 0, 1, async (s, e) => await ChooseStarter());

            AddButtonToGrid("Sümbol/värv", 1, 0, async (s, e) => await ChooseSymbolAndColor());

            botButton = new Button
            {
                Text = "Bot: VÄLJAS",
                FontSize = 14,
                HeightRequest = 45
            };
            botButton.Clicked += async (s, e) =>
            {
                settings.BotEnabled = !settings.BotEnabled;
                UpdateBotButton();
                await StartNewGameAsync();
            };
            buttonsGrid.Add(botButton, 1, 1);

            AddButtonToGrid("Välja suurus", 2, 0, async (s, e) => await ChooseBoardSize());
            AddButtonToGrid("Reeglid", 2, 1, async (s, e) => await Navigation.PushAsync(new RulesPage(settings)));

            AddButtonToGrid("Statistika", 3, 0, async (s, e) => await Navigation.PushAsync(new StatisticsPage(settings)));
            AddButtonToGrid("Seaded", 3, 1, async (s, e) =>
            {
                await Navigation.PushAsync(new SettingsPage(settings));
                ApplySettings();
                await StartNewGameAsync();
            });
        }

        private void AddButtonToGrid(string text, int row, int column, EventHandler clicked)
        {
            Button btn = new Button
            {
                Text = text,
                FontSize = 14,
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.Fill
            };
            btn.Clicked += clicked;
            buttonsGrid.Add(btn, column, row);
        }

        private Grid Tee_grid3X3()
        {
            int size = settings.BoardSize;
            bool isDark = settings.DarkTheme;

            Grid grid = new Grid
            {
                WidthRequest = 330,
                HeightRequest = 330,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                RowSpacing = 4,
                ColumnSpacing = 4,
                BackgroundColor = isDark ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#D0D0D0")
            };

            gameButtons = new Button[size, size];

            for (int i = 0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Button kast = new Button
                    {
                        Text = "",
                        FontSize = size == 3 ? 42 : 28,
                        BackgroundColor = isDark ? Color.FromArgb("#2D2D2D") : Colors.White,
                        TextColor = isDark ? Colors.White : Colors.Black
                    };

                    int rida = r;
                    int veerg = c;

                    kast.Clicked += async (s, args) =>
                    {
                        await PlayerMove(rida, veerg);
                    };

                    gameButtons[r, c] = kast;
                    grid.Add(kast, c, r);
                }
            }

            return grid;
        }

        private async Task PlayerMove(int row, int column)
        {
            if (gameOver || (settings.BotEnabled && game.CurrentPlayer == "O")) return;

            bool success = game.MakeMove(row, column);
            if (!success) return;

            UpdateButton(row, column);

            if (await CheckGameEnd()) return;

            game.SwitchPlayer();
            UpdatePlayerLabel();

            if (settings.BotEnabled && game.CurrentPlayer == "O")
            {
                await BotMove();
            }
        }

        private async Task BotMove()
        {
            await Task.Delay(400);

            var move = game.GetBotMove();
            if (move.Row < 0) return;

            bool success = game.MakeMove(move.Row, move.Column);
            if (!success) return;

            UpdateButton(move.Row, move.Column);

            if (await CheckGameEnd()) return;

            game.SwitchPlayer();
            UpdatePlayerLabel();
        }

        private void UpdateButton(int row, int column)
        {
            string player = game.CurrentPlayer;

            if (player == "X")
            {
                gameButtons[row, column].Text = settings.XSymbol;
                gameButtons[row, column].TextColor = Color.FromArgb(settings.XColor);
            }
            else
            {
                gameButtons[row, column].Text = settings.OSymbol;
                gameButtons[row, column].TextColor = Color.FromArgb(settings.OColor);
            }
        }

        private async Task<bool> CheckGameEnd()
        {
            if (game.CheckWinner())
            {
                gameOver = true;
                DrawWinningLine();

                string symbol = game.CurrentPlayer == "X" ? settings.XSymbol : settings.OSymbol;
                await StatisticsManager.AddWinAsync(game.CurrentPlayer);

                bool answer = await DisplayAlertAsync("Mäng läbi", symbol + " võitis! Kas soovid veel mängida?", "Jah", "Ei");

                if (answer) await StartNewGameAsync();
                return true;
            }

            if (game.IsDraw())
            {
                gameOver = true;
                await StatisticsManager.AddDrawAsync();

                bool answer = await DisplayAlertAsync("Mäng läbi", "Mäng jäi viiki! Kas soovid veel mängida?", "Jah", "Ei");

                if (answer) await StartNewGameAsync();
                return true;
            }

            return false;
        }

        private void StartNewGame()
        {
            _ = StartNewGameAsync();
        }

        private async Task StartNewGameAsync()
        {
            gameOver = false;
            winningLineDrawable.IsVisible = false;
            winningLineView.Invalidate();

            game = new GameLogic(settings.BoardSize);
            string starter = GetStarter();
            game.Reset(starter);

            sizeLabel.Text = "Mänguväli: " + settings.BoardSize + " × " + settings.BoardSize;

            Grid newGrid = Tee_grid3X3();

            boardContainer.Children.Clear();
            boardContainer.Add(newGrid);
            boardContainer.Add(winningLineView);

            gameGrid = newGrid;
            UpdatePlayerLabel();

            if (settings.BotEnabled && game.CurrentPlayer == "O")
            {
                await BotMove();
            }
        }

        private string GetStarter()
        {
            if (settings.Starter == "X") return "X";
            if (settings.Starter == "O") return "O";

            int number = rnd.Next(0, 2);
            return number == 0 ? "X" : "O";
        }

        private void UpdatePlayerLabel()
        {
            string symbol = game.CurrentPlayer == "X" ? settings.XSymbol : settings.OSymbol;
            playerLabel.Text = "Mängija " + symbol + " kord";
        }

        private void UpdateBotButton()
        {
            if (botButton != null)
            {
                botButton.Text = settings.BotEnabled ? "Bot: SEES" : "Bot: VÄLJAS";
            }
        }

        private async Task ChooseStarter()
        {
            string result = await DisplayActionSheetAsync("Kes alustab?", "Tühista", null, settings.XSymbol, settings.OSymbol, "Juhuslik");

            if (result == settings.XSymbol) { settings.Starter = "X"; await StartNewGameAsync(); }
            else if (result == settings.OSymbol) { settings.Starter = "O"; await StartNewGameAsync(); }
            else if (result == "Juhuslik") { settings.Starter = "Random"; await StartNewGameAsync(); }
        }

        private async Task ChooseBoardSize()
        {
            string result = await DisplayActionSheetAsync("Vali mänguvälja suurus", "Tühista", null, "3 × 3", "4 × 4", "5 × 5");

            if (result == "3 × 3") settings.BoardSize = 3;
            else if (result == "4 × 4") settings.BoardSize = 4;
            else if (result == "5 × 5") settings.BoardSize = 5;
            else return;

            await StartNewGameAsync();
        }

        private async Task ChooseSymbolAndColor()
        {
            string symbolResult = await DisplayActionSheetAsync("Vali sümbolid", "Tühista", null, "X ja O", "♥ ja ○", "★ ja ●");

            if (symbolResult == "X ja O") { settings.XSymbol = "X"; settings.OSymbol = "O"; }
            else if (symbolResult == "♥ ja ○") { settings.XSymbol = "♥"; settings.OSymbol = "○"; }
            else if (symbolResult == "★ ja ●") { settings.XSymbol = "★"; settings.OSymbol = "●"; }
            else return;

            string colorResult = await DisplayActionSheetAsync("Vali värvid", "Tühista", null, "Roosa ja sinine", "Punane ja roheline", "Lilla ja oranž");

            if (colorResult == "Roosa ja sinine") { settings.XColor = "#D85B7F"; settings.OColor = "#4D78C2"; }
            else if (colorResult == "Punane ja roheline") { settings.XColor = "#D64545"; settings.OColor = "#45A66F"; }
            else if (colorResult == "Lilla ja oranž") { settings.XColor = "#8057B8"; settings.OColor = "#E68A3C"; }

            await StartNewGameAsync();
        }

        private void ApplySettings()
        {
            bool isDark = settings.DarkTheme;

            Application.Current!.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;

            BackgroundColor = isDark ? Color.FromArgb("#121212") : Color.FromArgb("#FAF7F2");

            Color textColor = isDark ? Colors.White : Color.FromArgb("#222222");

            if (playerLabel != null) playerLabel.TextColor = textColor;
            if (sizeLabel != null) sizeLabel.TextColor = textColor;

            UpdateBotButton();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplySettings();

            if (gameGrid != null)
            {
                _ = StartNewGameAsync();
            }
        }

        private void DrawWinningLine()
        {
            winningLineDrawable.IsVisible = true;

            int size = settings.BoardSize;
            float boardSize = 330;
            float cellSize = boardSize / size;

            List<(int Row, int Column)> winningCells = game.GetWinningCells(game.CurrentPlayer);

            if (winningCells.Count == 0) return;

            var first = winningCells[0];
            var last = winningCells[winningCells.Count - 1];

            float startX = first.Column * cellSize + cellSize / 2;
            float startY = first.Row * cellSize + cellSize / 2;
            float endX = last.Column * cellSize + cellSize / 2;
            float endY = last.Row * cellSize + cellSize / 2;

            winningLineDrawable.StartX = startX;
            winningLineDrawable.StartY = startY;
            winningLineDrawable.EndX = endX;
            winningLineDrawable.EndY = endY;

            winningLineView.Invalidate();
        }
    }
}