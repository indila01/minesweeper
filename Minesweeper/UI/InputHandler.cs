using System;
using System.Text.RegularExpressions;

namespace Minesweeper.UI
{
    /// <summary>
    /// Handles user input for the Minesweeper game.
    /// </summary>
    public class InputHandler
    {
        private static readonly Regex CellRegex = new Regex(@"^([A-Za-z])(\d+)$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the board size from the user.
        /// </summary>
        /// <returns>The board size.</returns>
        public static int GetBoardSize()
        {
            int size = 0;
            bool isValid = false;

            do
            {
                Console.Write("Enter the size of the grid (e.g. 4 for a 4x4 grid): \n");
                string? input = Console.ReadLine();

                isValid = !string.IsNullOrEmpty(input) && int.TryParse(input, out size) && size > 0 && size <= 26;

                if (!isValid)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer between 1 and 26.");
                }
            } while (!isValid);

            return size;
        }

        /// <summary>
        /// Gets the number of mines from the user.
        /// </summary>
        /// <param name="maxMines">The maximum number of mines allowed.</param>
        /// <returns>The number of mines.</returns>
        public static int GetMineCount(int maxMines)
        {
            int mines = 0;
            bool isValid = false;

            do
            {
                Console.Write($"Enter the number of mines to place on the grid (maximum is {maxMines}): \n");
                string? input = Console.ReadLine();

                isValid = !string.IsNullOrEmpty(input) && int.TryParse(input, out mines) && mines > 0 && mines <= maxMines;

                if (!isValid)
                {
                    Console.WriteLine($"Invalid input. Please enter a positive integer between 1 and {maxMines}.");
                }
            } while (!isValid);

            return mines;
        }

        /// <summary>
        /// Gets the cell coordinates from the user.
        /// </summary>
        /// <param name="boardSize">The size of the board.</param>
        /// <param name="row">The row of the selected cell.</param>
        /// <param name="column">The column of the selected cell.</param>
        /// <returns>True if the input was valid, false otherwise.</returns>
        public static bool GetCellCoordinates(int boardSize, out int row, out int column)
        {
            row = -1;
            column = -1;

            Console.Write("Select a square to reveal (e.g. A1): ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            Match match = CellRegex.Match(input);

            if (!match.Success)
            {
                Console.WriteLine("Invalid input format. Please use the format [Letter][Number], e.g. A1.");
                return false;
            }

            char rowChar = char.ToUpper(match.Groups[1].Value[0]);
            if (!int.TryParse(match.Groups[2].Value, out column) || column < 1 || column > boardSize)
            {
                Console.WriteLine($"Column must be between 1 and {boardSize}.");
                return false;
            }

            row = rowChar - 'A';
            column--; // Convert to 0-based index

            if (row < 0 || row >= boardSize)
            {
                Console.WriteLine($"Row must be between A and {(char)('A' + boardSize - 1)}.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Waits for the user to press any key to continue.
        /// </summary>
        public static void WaitForKeyPress()
        {
            Console.WriteLine("Press any key to play again...");
            Console.ReadKey(true);
        }
    }
} 