using System;

namespace Minesweeper.Models
{
    /// <summary>
    /// Represents the Minesweeper game and manages the game flow.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets the board for this game.
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the game is over.
        /// </summary>
        public bool IsGameOver => Board.IsGameLost || IsGameWon;

        /// <summary>
        /// Gets a value indicating whether the game has been won.
        /// </summary>
        public bool IsGameWon => Board.CheckWin();

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        /// <param name="mineCount">The number of mines to place on the board.</param>
        /// <param name="randomSeed">Optional seed for the random number generator.</param>
        public Game(int size, int mineCount, int? randomSeed = null)
        {
            ValidateGameParameters(size, mineCount);
            Board = new Board(size, mineCount, randomSeed);
        }

        /// <summary>
        /// Validates the game parameters.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        /// <param name="mineCount">The number of mines to place on the board.</param>
        private void ValidateGameParameters(int size, int mineCount)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Board size must be greater than 0", nameof(size));
            }

            int maxMines = (int)(size * size * 0.35); // Maximum 35% of cells can be mines
            if (mineCount <= 0 || mineCount > maxMines)
            {
                throw new ArgumentException($"Mine count must be between 1 and {maxMines}", nameof(mineCount));
            }
        }

        /// <summary>
        /// Processes a player's move to reveal a cell.
        /// </summary>
        /// <param name="row">The row of the cell to reveal.</param>
        /// <param name="col">The column of the cell to reveal.</param>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool RevealCell(int row, int col)
        {
            // Check if game is already over
            if (IsGameOver)
            {
                return false;
            }

            // Attempt to reveal the cell on the board
            bool success = Board.RevealCell(row, col);

            // Return the result
            return success;
        }

        /// <summary>
        /// Processes a player's move to toggle a flag on a cell.
        /// </summary>
        /// <param name="row">The row of the cell to flag.</param>
        /// <param name="col">The column of the cell to flag.</param>
        /// <returns>True if the flag was toggled, false otherwise.</returns>
        public bool ToggleFlag(int row, int col)
        {
            // Check if game is already over
            if (IsGameOver)
            {
                return false;
            }

            // Attempt to toggle the flag on the board
            return Board.ToggleFlag(row, col);
        }

        /// <summary>
        /// Starts a new game with the same size and mine count.
        /// </summary>
        public void NewGame()
        {
            Board = new Board(Board.Size, Board.MineCount);
        }

        /// <summary>
        /// Starts a new game with different parameters.
        /// </summary>
        /// <param name="size">The size of the new game board.</param>
        /// <param name="mineCount">The number of mines to place on the new board.</param>
        public void NewGame(int size, int mineCount)
        {
            ValidateGameParameters(size, mineCount);
            Board = new Board(size, mineCount);
        }
    }
} 