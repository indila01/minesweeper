using System;
using Xunit;
using Minesweeper.Models;
using System.Reflection;

namespace Minesweeper.Tests.Models
{
    public class GameTests
    {
        [Fact]
        public void Constructor_WithValidParameters_CreatesGame()
        {
            // Arrange & Act
            var game = new Game(4, 3);

            // Assert
            Assert.NotNull(game.Board);
            Assert.Equal(4, game.Board.Size);
            Assert.Equal(3, game.Board.MineCount);
            Assert.False(game.IsGameOver);
            Assert.False(game.IsGameWon);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        public void Constructor_WithInvalidSize_ThrowsArgumentException(int size, int mineCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Game(size, mineCount));
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(4, -1)]
        [InlineData(4, 6)] // 6 exceeds 35% of 16 cells (5.6)
        public void Constructor_WithInvalidMineCount_ThrowsArgumentException(int size, int mineCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Game(size, mineCount));
        }

        [Fact]
        public void NewGame_WithSameParameters_CreatesNewBoard()
        {
            // Arrange
            var game = new Game(4, 3);
            var originalBoard = game.Board;

            // Reveal a cell to change the state
            game.RevealCell(0, 0);

            // Act
            game.NewGame();

            // Assert
            Assert.NotSame(originalBoard, game.Board);
            Assert.Equal(4, game.Board.Size);
            Assert.Equal(3, game.Board.MineCount);
            Assert.False(game.IsGameOver);
        }

        [Fact]
        public void NewGame_WithDifferentParameters_CreatesNewBoardWithNewParameters()
        {
            // Arrange
            var game = new Game(4, 3);

            // Act
            game.NewGame(5, 4);

            // Assert
            Assert.Equal(5, game.Board.Size);
            Assert.Equal(4, game.Board.MineCount);
            Assert.False(game.IsGameOver);
        }

        [Fact]
        public void RevealCell_AfterGameIsLost_ReturnsFalse()
        {
            // Arrange
            var game = new Game(3, 1);
            ForceGameLost(game);
            
            // Act
            bool result = game.RevealCell(0, 0);
            
            // Assert
            Assert.True(game.IsGameOver);
            Assert.False(result);
        }

        [Fact]
        public void RevealCell_AfterGameIsWon_ReturnsFalse()
        {
            // Arrange
            var game = new Game(3, 1);
            ForceGameWon(game);
            
            // Act
            bool result = game.RevealCell(0, 0);
            
            // Assert
            Assert.True(game.IsGameOver);
            Assert.True(game.IsGameWon);
            Assert.False(result);
        }

        [Fact]
        public void ToggleFlag_AfterGameIsOver_ReturnsFalse()
        {
            // Arrange
            var game = new Game(3, 1);
            ForceGameLost(game);
            
            // Act
            bool result = game.ToggleFlag(0, 0);
            
            // Assert
            Assert.True(game.IsGameOver);
            Assert.False(result);
        }
        
        // Helper method to force the game into a lost state
        private void ForceGameLost(Game game)
        {
            // Use reflection to set IsGameLost to true
            var isGameLostProperty = typeof(Board).GetProperty("IsGameLost");
            isGameLostProperty.SetValue(game.Board, true);
        }
        
        // Helper method to force the game into a won state
        private void ForceGameWon(Game game)
        {
            // Set up a game state where all non-mine cells are revealed
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    game.Board.Grid[row, col].HasMine = false;
                    game.Board.Grid[row, col].IsRevealed = true;
                }
            }
            
            // Place one mine and leave it unrevealed
            game.Board.Grid[0, 0].HasMine = true;
            game.Board.Grid[0, 0].IsRevealed = false;
        }
    }
} 