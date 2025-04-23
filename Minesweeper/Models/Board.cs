using System;
using System.Collections.Generic;

namespace Minesweeper.Models
{
    /// <summary>
    /// Represents the Minesweeper game board.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Gets the size of the board (board is always square).
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets the number of mines on the board.
        /// </summary>
        public int MineCount { get; private set; }

        /// <summary>
        /// Gets the grid of cells that make up the board.
        /// </summary>
        public Cell[,] Grid { get; }

        /// <summary>
        /// Gets a value indicating whether the game has been lost.
        /// </summary>
        public bool IsGameLost { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the first move has been made.
        /// </summary>
        public bool IsFirstMove { get; private set; }

        /// <summary>
        /// Gets the random number generator used for mine placement.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="size">The size of the board.</param>
        /// <param name="mineCount">The number of mines to place on the board.</param>
        /// <param name="randomSeed">Optional seed for the random number generator.</param>
        public Board(int size, int mineCount, int? randomSeed = null)
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

            Size = size;
            MineCount = mineCount;
            IsGameLost = false;
            IsFirstMove = true;
            _random = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();

            // Initialize the grid in the constructor
            Grid = new Cell[Size, Size];
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col] = new Cell(row, col);
                }
            }
        }

        /// <summary>
        /// Places mines on the board, avoiding the given cell.
        /// </summary>
        /// <param name="safeRow">The row of the cell to avoid placing a mine on.</param>
        /// <param name="safeCol">The column of the cell to avoid placing a mine on.</param>
        public void PlaceMines(int safeRow, int safeCol)
        {
            // Reset any previously placed mines (in case of a restart)
            foreach (var cell in Grid)
            {
                cell.HasMine = false;
                cell.AdjacentMines = 0;
            }

            int minesPlaced = 0;
            while (minesPlaced < MineCount)
            {
                int row = _random.Next(Size);
                int col = _random.Next(Size);

                // Skip the safe cell and cells that already have mines
                if ((row == safeRow && col == safeCol) || Grid[row, col].HasMine)
                {
                    continue;
                }

                Grid[row, col].HasMine = true;
                minesPlaced++;
            }

            CalculateAdjacentMines();
        }

        /// <summary>
        /// Calculates the number of adjacent mines for each cell.
        /// </summary>
        private void CalculateAdjacentMines()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (Grid[row, col].HasMine)
                    {
                        continue;
                    }

                    Grid[row, col].AdjacentMines = CountAdjacentMines(row, col);
                }
            }
        }

        /// <summary>
        /// Counts the number of mines adjacent to the given cell.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        /// <returns>The number of adjacent mines.</returns>
        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;

            for (int r = Math.Max(0, row - 1); r <= Math.Min(Size - 1, row + 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(Size - 1, col + 1); c++)
                {
                    if ((r != row || c != col) && Grid[r, c].HasMine)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Reveals the cell at the given coordinates.
        /// </summary>
        /// <param name="row">The row of the cell to reveal.</param>
        /// <param name="col">The column of the cell to reveal.</param>
        /// <returns>True if the move was successful, false if it was invalid.</returns>
        public bool RevealCell(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return false; // Invalid coordinates
            }

            var cell = Grid[row, col];

            if (cell.IsRevealed || cell.IsFlagged)
            {
                return false; // Cell already revealed or flagged
            }

            if (IsFirstMove)
            {
                IsFirstMove = false;
                PlaceMines(row, col); // Place mines, ensuring the first revealed cell is safe
            }

            cell.IsRevealed = true;

            if (cell.HasMine)
            {
                IsGameLost = true; // Set game as lost if a mine is revealed
                return true;
            }

            if (cell.AdjacentMines == 0)
            {
                RevealAdjacentCells(row, col);
            }

            return true;
        }

        /// <summary>
        /// Recursively reveals all adjacent cells with zero adjacent mines.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        private void RevealAdjacentCells(int row, int col)
        {
            for (int r = Math.Max(0, row - 1); r <= Math.Min(Size - 1, row + 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(Size - 1, col + 1); c++)
                {
                    var adjacentCell = Grid[r, c];
                    
                    if (!adjacentCell.IsRevealed && !adjacentCell.IsFlagged && !adjacentCell.HasMine)
                    {
                        adjacentCell.IsRevealed = true;
                        
                        if (adjacentCell.AdjacentMines == 0)
                        {
                            RevealAdjacentCells(r, c);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the flag on a cell.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        /// <returns>True if the flag was toggled, false if the cell is already revealed.</returns>
        public bool ToggleFlag(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return false; // Invalid coordinates
            }

            var cell = Grid[row, col];

            if (cell.IsRevealed)
            {
                return false; // Can't flag a revealed cell
            }

            cell.IsFlagged = !cell.IsFlagged;
            return true;
        }

        /// <summary>
        /// Checks if the game has been won.
        /// </summary>
        /// <returns>True if all non-mine cells have been revealed, false otherwise.</returns>
        public bool CheckWin()
        {
            if (IsGameLost)
            {
                return false;
            }

            foreach (var cell in Grid)
            {
                if (!cell.HasMine && !cell.IsRevealed)
                {
                    return false; // Still have non-mine cells to reveal
                }
            }

            return true;
        }
    }
} 