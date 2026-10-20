using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

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

            game = new GameLogic(
                settings.BoardSize);

            gameButtons = new Button[
                settings.BoardSize,
                settings.BoardSize];

            

            BuildPage();
            StartNewGame();
        }

        private void BuildPage()
        {
            Title = "Trips-Traps-Trull";
            // Ülemine topelt-tiitel on eemaldatud, alles jäi ainult mängu info
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

            winningLineDrawable =
                new WinningLineDrawable();

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
                WidthRequest = 330,
                HeightRequest = 330,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            boardContainer.Add(gameGrid);
            boardContainer.Add(winningLineView);

            VerticalStackLayout layout =
                new VerticalStackLayout
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
                ColumnSpacing = 15,
                RowSpacing = 15
            };

            buttonsGrid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

            buttonsGrid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

            for (int i = 0; i < 4; i++)
            {
                buttonsGrid.RowDefinitions.Add(
                    new RowDefinition
                    {
                        Height = GridLength.Auto
                    });
            }

            Button newGameButton = new Button
            {
                Text = "Uus mäng",
                FontSize = 17
            };
            newGameButton.Clicked += (sender, e) => { StartNewGame(); };
            buttonsGrid.Add(newGameButton, 0, 0);

            Button starterButton = new Button
            {
                Text = "Kes alustab?",
                FontSize = 17
            };
            starterButton.Clicked += async (sender, e) => { await ChooseStarter(); };
            buttonsGrid.Add(starterButton, 1, 0);

            Button symbolButton = new Button
            {
                Text = "Sümbol/värv",
                FontSize = 17
            };
            symbolButton.Clicked += async (sender, e) => { await ChooseSymbolAndColor(); };
            buttonsGrid.Add(symbolButton, 2, 0);

            botButton = new Button
            {
                Text = "Bot: VÄLJAS",
                FontSize = 17
            };
            botButton.Clicked += async (sender, e) =>
            {
                settings.BotEnabled = !settings.BotEnabled;
                UpdateBotButton();
                await StartNewGameAsync();
            };
            buttonsGrid.Add(botButton, 0, 1);

            Button sizeButton = new Button
            {
                Text = "Välja suurus",
                FontSize = 17
            };
            sizeButton.Clicked += async (sender, e) => { await ChooseBoardSize(); };
            buttonsGrid.Add(sizeButton, 1, 1);

            Button rulesButton = new Button
            {
                Text = "Reeglid",
                FontSize = 17
            };
            rulesButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new RulesPage());
            };
            buttonsGrid.Add(rulesButton, 2, 1);

            Button statisticsButton = new Button
            {
                Text = "Statistika",
                FontSize = 17
            };
            statisticsButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new StatisticsPage());
            };
            buttonsGrid.Add(statisticsButton, 0, 2);

            Button settingsButton = new Button
            {
                Text = "Seaded",
                FontSize = 17
            };
            settingsButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new SettingsPage(settings));
                ApplySettings();
                await StartNewGameAsync();
            };
            buttonsGrid.Add(settingsButton, 1, 2);
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
                RowSpacing = 5,
                ColumnSpacing = 5
            };

            gameButtons = new Button[size, size];

            for (int i = 0; i < size; i++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });

                grid.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
            }

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Button kast = new Button
                    {
                        Text = "",
                        FontSize = size == 3 ? 42 : 28,
                        // Tumeda teema korral ruudud tumedamad, heleda korral valged/kontrastsed
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

        private async System.Threading.Tasks.Task PlayerMove(int row, int column)
        {
            if (gameOver || (settings.BotEnabled && game.CurrentPlayer == "O"))
            {
                return;
            }

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

        private async System.Threading.Tasks.Task BotMove()
        {
            await System.Threading.Tasks.Task.Delay(400);

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

        private async System.Threading.Tasks.Task<bool> CheckGameEnd()
        {
            if (game.CheckWinner())
            {
                gameOver = true;
                DrawWinningLine();

                string symbol = game.CurrentPlayer == "X" ? settings.XSymbol : settings.OSymbol;
                StatisticsManager.AddWin(game.CurrentPlayer);

                bool answer = await DisplayAlertAsync(
                    "Mäng läbi",
                    symbol + " võitis! Kas soovid veel mängida?",
                    "Jah",
                    "Ei");

                if (answer)
                {
                    await StartNewGameAsync();
                }
                return true;
            }

            if (game.IsDraw())
            {
                gameOver = true;
                StatisticsManager.AddDraw();

                bool answer = await DisplayAlertAsync(
                    "Mäng läbi",
                    "Mäng jäi viiki! Kas soovid veel mängida?",
                    "Jah",
                    "Ei");

                if (answer)
                {
                    await StartNewGameAsync();
                }
                return true;
            }

            return false;
        }

        private void StartNewGame()
        {
            _ = StartNewGameAsync();
        }

        private async System.Threading.Tasks.Task StartNewGameAsync()
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
            if (settings.BotEnabled)
            {
                botButton.Text = "Mäng botiga: SEES";
            }
            else
            {
                botButton.Text = "Mäng botiga: VÄLJAS";
            }
        }

        private async System.Threading.Tasks.Task ChooseStarter()
        {
            string result = await DisplayActionSheetAsync(
                "Kes alustab?",
                "Tühista",
                null,
                settings.XSymbol,
                settings.OSymbol,
                "Juhuslik");

            if (result == settings.XSymbol)
            {
                settings.Starter = "X";
                await StartNewGameAsync();
            }
            else if (result == settings.OSymbol)
            {
                settings.Starter = "O";
                await StartNewGameAsync();
            }
            else if (result == "Juhuslik")
            {
                settings.Starter = "Random";
                await StartNewGameAsync();
            }
        }

        private async System.Threading.Tasks.Task ChooseBoardSize()
        {
            string result = await DisplayActionSheetAsync(
                "Vali mänguvälja suurus",
                "Tühista",
                null,
                "3 × 3",
                "4 × 4",
                "5 × 5");

            if (result == "3 × 3") settings.BoardSize = 3;
            else if (result == "4 × 4") settings.BoardSize = 4;
            else if (result == "5 × 5") settings.BoardSize = 5;
            else return;

            await StartNewGameAsync();
        }

        private async System.Threading.Tasks.Task ChooseSymbolAndColor()
        {
            string symbolResult = await DisplayActionSheetAsync(
                "Vali sümbolid",
                "Tühista",
                null,
                "X ja O",
                "♥ ja ○",
                "★ ja ●");

            if (symbolResult == "X ja O")
            {
                settings.XSymbol = "X";
                settings.OSymbol = "O";
            }
            else if (symbolResult == "♥ ja ○")
            {
                settings.XSymbol = "♥";
                settings.OSymbol = "○";
            }
            else if (symbolResult == "★ ja ●")
            {
                settings.XSymbol = "★";
                settings.OSymbol = "●";
            }
            else return;

            string colorResult = await DisplayActionSheetAsync(
                "Vali värvid",
                "Tühista",
                null,
                "Roosa ja sinine",
                "Punane ja roheline",
                "Lilla ja oranž");

            if (colorResult == "Roosa ja sinine")
            {
                settings.XColor = "#D85B7F";
                settings.OColor = "#4D78C2";
            }
            else if (colorResult == "Punane ja roheline")
            {
                settings.XColor = "#D64545";
                settings.OColor = "#45A66F";
            }
            else if (colorResult == "Lilla ja oranž")
            {
                settings.XColor = "#8057B8";
                settings.OColor = "#E68A3C";
            }

            await StartNewGameAsync();
        }

        private void ApplySettings()
        {
            // Mängulehe taust võtab seadistatud taustavärvi
            BackgroundColor = Color.FromArgb(settings.BackgroundColor);

            bool isDark = settings.DarkTheme;
            Color textColor = isDark ? Colors.White : Colors.Black;

            // Tekstid (mängija kord ja välja suurus) kohanduvad automaatselt teemaga
            if (playerLabel != null) playerLabel.TextColor = textColor;
            if (sizeLabel != null) sizeLabel.TextColor = textColor;

            Application.Current!.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;

            UpdateBotButton();
        }

        private void DrawWinningLine()
        {
            winningLineDrawable.IsVisible = true;

            int size = settings.BoardSize;
            float boardSize = 330;
            float cellSize = boardSize / size;

            List<(int Row, int Column)> winningCells =
                game.GetWinningCells(game.CurrentPlayer);

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