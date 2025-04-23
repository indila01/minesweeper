using System;
using System.Text;
using Minesweeper.Models;

namespace Minesweeper.UI
{
    /// <summary>
    /// Formats the output for the Minesweeper game.
    /// </summary>
    public class OutputFormatter
    {
        /// <summary>
        /// Displays the Minesweeper board.
        /// </summary>
        /// <param name="board">The game board to display.</param>
        /// <param name="showMines">Whether to show mines (for game over).</param>
        public static void DisplayBoard(Board board, bool showMines = false)
        {
            Console.WriteLine("\nHere is your minefield:");
            
            // Print column headers
            Console.Write("  ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write($"{col + 1} ");
            }
            Console.WriteLine();

            // Print rows
            for (int row = 0; row < board.Size; row++)
            {
                // Print row header (A, B, C, etc.)
                Console.Write($"{(char)('A' + row)} ");

                // Print cells
                for (int col = 0; col < board.Size; col++)
                {
                    Cell cell = board.Grid[row, col];
                    Console.Write(GetCellDisplay(cell, showMines) + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Gets the display character for a cell.
        /// </summary>
        /// <param name="cell">The cell to get the display for.</param>
        /// <param name="showMines">Whether to show mines.</param>
        /// <returns>The character representing the cell.</returns>
        private static char GetCellDisplay(Cell cell, bool showMines)
        {
            if (cell.IsFlagged)
            {
                return 'F';
            }

            if (!cell.IsRevealed)
            {
                return '_';
            }

            if (cell.HasMine)
            {
                return showMines ? '*' : '_';
            }

            // For revealed cells, show the number of adjacent mines or 0
            return cell.AdjacentMines > 0 ? cell.AdjacentMines.ToString()[0] : '0';
        }

        /// <summary>
        /// Displays the welcome message.
        /// </summary>
        public static void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to Minesweeper!");
        }

        /// <summary>
        /// Displays the game over message.
        /// </summary>
        /// <param name="isWin">Whether the game was won.</param>
        public static void DisplayGameOverMessage(bool isWin)
        {
            if (isWin)
            {
                Console.WriteLine("Congratulations, you have won the game!");
            }
            else
            {
                Console.WriteLine("Oh no, you detonated a mine! Game over.");
            }
        }

        /// <summary>
        /// Displays a message about the revealed cell.
        /// </summary>
        /// <param name="cell">The cell that was revealed.</param>
        public static void DisplayCellInfo(Cell cell)
        {
            if (cell.HasMine)
            {
                return; // Game will end, no need to show this
            }

            Console.WriteLine($"This square contains {cell.AdjacentMines} adjacent mines. \n");
        }
    }
} 