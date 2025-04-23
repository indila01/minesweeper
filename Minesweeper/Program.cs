using System;
using Minesweeper.Models;
using Minesweeper.UI;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain = true;

            while (playAgain)
            {
                OutputFormatter.DisplayWelcomeMessage();

                // Get game parameters from user
                int size = InputHandler.GetBoardSize();
                int maxMines = (int)(size * size * 0.35);
                int mines = InputHandler.GetMineCount(maxMines);

                // Create and start a new game
                Game game = new Game(size, mines);
                playAgain = RunGameLoop(game);
            }
        }

        private static bool RunGameLoop(Game game)
        {
            bool gameRunning = true;

            OutputFormatter.DisplayBoard(game.Board);

            while (gameRunning)
            {
                // Get user input
                if (!InputHandler.GetCellCoordinates(game.Board.Size, out int row, out int col))
                {
                    continue; // Invalid input, try again
                }

                // Process move
                bool moveSuccess = game.RevealCell(row, col);
                if (!moveSuccess)
                {
                    Console.WriteLine("Invalid move. Please try again.");
                    continue;
                }

                // Display cell info if not a mine
                Cell revealedCell = game.Board.Grid[row, col];
                OutputFormatter.DisplayCellInfo(revealedCell);

                // Check game state
                if (game.Board.IsGameLost)
                {
                    OutputFormatter.DisplayBoard(game.Board, true); // Show mines
                    OutputFormatter.DisplayGameOverMessage(false);
                    gameRunning = false;
                }
                else if (game.IsGameWon)
                {
                    OutputFormatter.DisplayBoard(game.Board);
                    OutputFormatter.DisplayGameOverMessage(true);
                    gameRunning = false;
                }
                else
                {
                    OutputFormatter.DisplayBoard(game.Board);
                }
            }

            InputHandler.WaitForKeyPress();
            return true; // Play again
        }
    }
}
