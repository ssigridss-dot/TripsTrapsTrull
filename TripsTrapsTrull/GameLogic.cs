using System;
using System.Collections.Generic;

namespace TripsTrapsTrull
{
    public class GameLogic
    {
        private Random rnd = new Random();

        public int BoardSize { get; private set; }

        public string?[,] Board { get; private set; }

        public string CurrentPlayer { get; private set; } = "X";


        public GameLogic(int boardSize)
        {
            BoardSize = boardSize;

            Board = new string?[BoardSize, BoardSize];
        }


        /// <summary>
        /// Alustab uue mängu.
        /// </summary>
        public void Reset(string starter)
        {
            Board = new string?[BoardSize, BoardSize];

            CurrentPlayer = starter;
        }


        /// <summary>
        /// Teeb käigu valitud lahtrisse.
        /// </summary>
        public bool MakeMove(int row, int column)
        {
            if (row < 0 ||
                row >= BoardSize ||
                column < 0 ||
                column >= BoardSize)
            {
                return false;
            }

            if (Board[row, column] != null)
            {
                return false;
            }

            Board[row, column] = CurrentPlayer;

            return true;
        }


        /// <summary>
        /// Vahetab mängijat.
        /// </summary>
        public void SwitchPlayer()
        {
            if (CurrentPlayer == "X")
            {
                CurrentPlayer = "O";
            }
            else
            {
                CurrentPlayer = "X";
            }
        }


        /// <summary>
        /// Kontrollib, kas praegune mängija võitis.
        /// </summary>
        public bool CheckWinner()
        {
            return CheckWinner(CurrentPlayer);
        }


        /// <summary>
        /// Kontrollib kindla mängija võitu.
        /// </summary>
        public bool CheckWinner(string player)
        {
            int[,] directions =
            {
                { 1, 0 },
                { 0, 1 },
                { 1, 1 },
                { 1, -1 }
            };


            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] != player)
                    {
                        continue;
                    }


                    for (int d = 0; d < directions.GetLength(0); d++)
                    {
                        int rowDirection = directions[d, 0];
                        int columnDirection = directions[d, 1];

                        int count = 1;

                        int newRow = row + rowDirection;
                        int newColumn = column + columnDirection;


                        while (
                            newRow >= 0 &&
                            newRow < BoardSize &&
                            newColumn >= 0 &&
                            newColumn < BoardSize &&
                            Board[newRow, newColumn] == player)
                        {
                            count++;

                            newRow += rowDirection;
                            newColumn += columnDirection;
                        }


                        if (count >= BoardSize)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public List<(int Row, int Column)> GetWinningCells(
    string player)
        {
            int[,] directions =
            {
        { 1, 0 },
        { 0, 1 },
        { 1, 1 },
        { 1, -1 }
    };


            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] != player)
                    {
                        continue;
                    }


                    for (int d = 0;
                         d < directions.GetLength(0);
                         d++)
                    {
                        int rowDirection =
                            directions[d, 0];

                        int columnDirection =
                            directions[d, 1];


                        List<(int Row, int Column)> cells =
                            new List<(int Row, int Column)>();


                        int currentRow = row;
                        int currentColumn = column;


                        while (
                            currentRow >= 0 &&
                            currentRow < BoardSize &&
                            currentColumn >= 0 &&
                            currentColumn < BoardSize &&
                            Board[currentRow, currentColumn] == player)
                        {
                            cells.Add(
                                (currentRow, currentColumn));


                            currentRow += rowDirection;
                            currentColumn += columnDirection;
                        }


                        if (cells.Count >= BoardSize)
                        {
                            return cells;
                        }
                    }
                }
            }


            return new List<(int Row, int Column)>();
        }


        /// <summary>
        /// Kontrollib, kas kõik lahtrid on täidetud.
        /// </summary>
        public bool IsDraw()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Leiab botile sobiva käigu.
        /// Bot proovib kõigepealt võita,
        /// seejärel vastase käiku blokeerida.
        /// </summary>
        public (int Row, int Column) GetBotMove()
        {
            string botPlayer = CurrentPlayer;

            string opponent;

            if (botPlayer == "X")
            {
                opponent = "O";
            }
            else
            {
                opponent = "X";
            }


            // 1. Proovime võita
            var winningMove = FindWinningMove(botPlayer);

            if (winningMove.HasValue)
            {
                return winningMove.Value;
            }


            // 2. Proovime vastast blokeerida
            var blockingMove = FindWinningMove(opponent);

            if (blockingMove.HasValue)
            {
                return blockingMove.Value;
            }


            // 3. Proovime võtta keskkoha
            int center = BoardSize / 2;

            if (Board[center, center] == null)
            {
                return (center, center);
            }


            // 4. Kui muu ei sobi, valime juhusliku vaba koha
            List<(int Row, int Column)> emptyCells =
                new List<(int Row, int Column)>();


            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] == null)
                    {
                        emptyCells.Add((row, column));
                    }
                }
            }


            if (emptyCells.Count > 0)
            {
                int index = rnd.Next(emptyCells.Count);

                return emptyCells[index];
            }


            return (-1, -1);
        }


        /// <summary>
        /// Leiab käigu, millega antud mängija saaks võita.
        /// </summary>
        private (int Row, int Column)? FindWinningMove(string player)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] != null)
                    {
                        continue;
                    }


                    Board[row, column] = player;


                    bool wins = CheckWinner(player);


                    Board[row, column] = null;


                    if (wins)
                    {
                        return (row, column);
                    }
                }
            }


            return null;
        }
    }
}