using System;
using Xunit;
using Minesweeper.Models;
using System.Reflection;

namespace Minesweeper.Tests.Models
{
    public class BoardTests
    {
        [Fact]
        public void Constructor_WithValidParameters_CreatesBoard()
        {
            // Arrange & Act
            var board = new Board(4, 3);

            // Assert
            Assert.Equal(4, board.Size);
            Assert.Equal(3, board.MineCount);
            Assert.NotNull(board.Grid);
            Assert.Equal(4, board.Grid.GetLength(0));
            Assert.Equal(4, board.Grid.GetLength(1));
            Assert.True(GetIsFirstMove(board));
            Assert.False(board.IsGameLost);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        public void Constructor_WithInvalidSize_ThrowsArgumentException(int size, int mineCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Board(size, mineCount));
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(4, -1)]
        [InlineData(4, 6)] // 6 exceeds 35% of 16 cells (5.6)
        public void Constructor_WithInvalidMineCount_ThrowsArgumentException(int size, int mineCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Board(size, mineCount));
        }

        [Fact]
        public void PlaceMines_PlacesCorrectNumberOfMines()
        {
            // Arrange
            var board = new Board(4, 3, 42); // Use fixed seed for deterministic test
            
            // Act
            board.PlaceMines(0, 0); // Keep (0,0) safe
            
            // Assert
            int mineCount = 0;
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Grid[row, col].HasMine)
                    {
                        mineCount++;
                    }
                }
            }
            
            Assert.Equal(3, mineCount);
            Assert.False(board.Grid[0, 0].HasMine); // Safe cell should not have mine
        }

        [Fact]
        public void RevealCell_WithMine_SetsGameLost()
        {
            // Arrange
            var board = new Board(4, 3, 42); // Use fixed seed for deterministic test
            
            // Initialize the board with a first move to avoid the safe cell mechanism
            board.RevealCell(0, 0);
            
            // Reset board state for test but keep mines in place
            foreach (var cell in board.Grid)
            {
                cell.IsRevealed = false;
            }
            
            // Find a cell with a mine
            int mineRow = -1, mineCol = -1;
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Grid[row, col].HasMine)
                    {
                        mineRow = row;
                        mineCol = col;
                        break;
                    }
                }
                if (mineRow >= 0) break;
            }
            
            // Ensure we found a mine
            Assert.True(mineRow >= 0 && mineCol >= 0, "No mine found on the board");
            
            // Act - Reveal the mine
            bool result = board.RevealCell(mineRow, mineCol);
            
            // Assert
            Assert.True(result, "RevealCell should return true for a valid move");
            Assert.True(board.IsGameLost, "Game should be lost after revealing a mine");
            Assert.True(board.Grid[mineRow, mineCol].IsRevealed, "Mine cell should be revealed");
        }

        [Fact]
        public void RevealCell_WithNoAdjacentMines_RevealsAdjacentCells()
        {
            // Arrange
            // Use a fixed board with known empty cells
            var board = new Board(3, 1, 42); // Small board with 1 mine for simpler test
            
            // Force mine placement
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    board.Grid[row, col].HasMine = false;
                }
            }
            
            board.Grid[2, 2].HasMine = true; // Place a mine in the bottom-right corner
            
            // Reveal a cell to set IsFirstMove to false
            board.RevealCell(0, 0);
            
            // Reset cell state for the actual test
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    board.Grid[row, col].IsRevealed = false;
                }
            }
            
            // Calculate adjacent mines for all cells
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (!board.Grid[row, col].HasMine)
                    {
                        int count = 0;
                        for (int r = Math.Max(0, row - 1); r <= Math.Min(board.Size - 1, row + 1); r++)
                        {
                            for (int c = Math.Max(0, col - 1); c <= Math.Min(board.Size - 1, col + 1); c++)
                            {
                                if ((r != row || c != col) && board.Grid[r, c].HasMine)
                                {
                                    count++;
                                }
                            }
                        }
                        board.Grid[row, col].AdjacentMines = count;
                    }
                }
            }
            
            // Act - Reveal a cell with no adjacent mines (0,0)
            bool result = board.RevealCell(0, 0);
            
            // Assert
            Assert.True(result);
            Assert.True(board.Grid[0, 0].IsRevealed);
            
            // Check that some adjacent cells were also revealed
            bool someAdjacentRevealed = false;
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if ((row != 0 || col != 0) && board.Grid[row, col].IsRevealed)
                    {
                        someAdjacentRevealed = true;
                        break;
                    }
                }
                if (someAdjacentRevealed) break;
            }
            
            Assert.True(someAdjacentRevealed);
        }

        [Fact]
        public void CheckWin_AllNonMineCellsRevealed_ReturnsTrue()
        {
            // Arrange
            var board = new Board(3, 1);
            
            // Force a specific configuration
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    board.Grid[row, col].HasMine = false;
                    board.Grid[row, col].IsRevealed = true;
                }
            }
            
            // Place one mine and make it not revealed
            board.Grid[2, 2].HasMine = true;
            board.Grid[2, 2].IsRevealed = false;
            
            // Act
            bool isWin = board.CheckWin();
            
            // Assert
            Assert.True(isWin);
        }

        [Fact]
        public void CheckWin_NotAllNonMineCellsRevealed_ReturnsFalse()
        {
            // Arrange
            var board = new Board(3, 1);
            
            // Force a specific configuration
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    board.Grid[row, col].HasMine = false;
                    board.Grid[row, col].IsRevealed = false;
                }
            }
            
            board.Grid[2, 2].HasMine = true;
            
            // Reveal some but not all non-mine cells
            board.Grid[0, 0].IsRevealed = true;
            
            // Act
            bool isWin = board.CheckWin();
            
            // Assert
            Assert.False(isWin);
        }

        [Fact]
        public void ToggleFlag_OnUnrevealedCell_TogglesFlag()
        {
            // Arrange
            var board = new Board(3, 1);
            
            // Act
            bool result = board.ToggleFlag(1, 1);
            
            // Assert
            Assert.True(result);
            Assert.True(board.Grid[1, 1].IsFlagged);
            
            // Act again to toggle off
            result = board.ToggleFlag(1, 1);
            
            // Assert
            Assert.True(result);
            Assert.False(board.Grid[1, 1].IsFlagged);
        }

        [Fact]
        public void ToggleFlag_OnRevealedCell_ReturnsFalse()
        {
            // Arrange
            var board = new Board(3, 1);
            board.Grid[1, 1].IsRevealed = true;
            
            // Act
            bool result = board.ToggleFlag(1, 1);
            
            // Assert
            Assert.False(result);
            Assert.False(board.Grid[1, 1].IsFlagged);
        }

        // Helper method to access private IsFirstMove property for testing
        private bool GetIsFirstMove(Board board)
        {
            var property = typeof(Board).GetProperty("IsFirstMove", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return property != null && (bool)property.GetValue(board);
        }
    }
} 