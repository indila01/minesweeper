using System;

namespace Minesweeper.Models
{
    /// <summary>
    /// Represents a single cell in the Minesweeper grid.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Gets or sets a value indicating whether this cell contains a mine.
        /// </summary>
        public bool HasMine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cell has been revealed by the player.
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cell has been flagged by the player.
        /// </summary>
        public bool IsFlagged { get; set; }

        /// <summary>
        /// Gets or sets the number of mines adjacent to this cell.
        /// </summary>
        public int AdjacentMines { get; set; }

        /// <summary>
        /// Gets or sets the row index of this cell in the grid.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the column index of this cell in the grid.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="column">The column index of the cell.</param>
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            HasMine = false;
            IsRevealed = false;
            IsFlagged = false;
            AdjacentMines = 0;
        }
    }
} 