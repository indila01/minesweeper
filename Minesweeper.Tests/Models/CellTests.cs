using System;
using Xunit;
using Minesweeper.Models;

namespace Minesweeper.Tests.Models
{
    public class CellTests
    {
        [Fact]
        public void Constructor_SetsInitialValues()
        {
            // Arrange & Act
            var cell = new Cell(1, 2);

            // Assert
            Assert.Equal(1, cell.Row);
            Assert.Equal(2, cell.Column);
            Assert.False(cell.HasMine);
            Assert.False(cell.IsRevealed);
            Assert.False(cell.IsFlagged);
            Assert.Equal(0, cell.AdjacentMines);
        }

        [Fact]
        public void HasMine_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var cell = new Cell(0, 0);

            // Act
            cell.HasMine = true;

            // Assert
            Assert.True(cell.HasMine);
        }

        [Fact]
        public void IsRevealed_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var cell = new Cell(0, 0);

            // Act
            cell.IsRevealed = true;

            // Assert
            Assert.True(cell.IsRevealed);
        }

        [Fact]
        public void IsFlagged_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var cell = new Cell(0, 0);

            // Act
            cell.IsFlagged = true;

            // Assert
            Assert.True(cell.IsFlagged);
        }

        [Fact]
        public void AdjacentMines_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var cell = new Cell(0, 0);

            // Act
            cell.AdjacentMines = 5;

            // Assert
            Assert.Equal(5, cell.AdjacentMines);
        }
    }
} 