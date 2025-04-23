using System;
using System.IO;
using Xunit;
using Minesweeper.Models;
using Minesweeper.UI;

namespace Minesweeper.Tests.IntegrationTests
{
    public class GameIntegrationTests
    {
        [Fact]
        public void Game_RevealingAllNonMineCells_ResultsInWin()
        {
            // Arrange
            Game game = new Game(3, 1);
            
            // Force mine placement
            game.RevealCell(0, 0); // Trigger the first move to place mines
            
            // Reset the board state - keep mines but clear revealed cells
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    game.Board.Grid[row, col].IsRevealed = false;
                }
            }
            
            // Place a mine manually in a specific location
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    game.Board.Grid[row, col].HasMine = false;
                }
            }
            
            // Place a single mine at position 2,2
            game.Board.Grid[2, 2].HasMine = true;
            
            // Update adjacent mine counts
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    if (!game.Board.Grid[row, col].HasMine)
                    {
                        int count = 0;
                        // Count adjacent mines
                        for (int r = Math.Max(0, row - 1); r <= Math.Min(game.Board.Size - 1, row + 1); r++)
                        {
                            for (int c = Math.Max(0, col - 1); c <= Math.Min(game.Board.Size - 1, col + 1); c++)
                            {
                                if ((r != row || c != col) && game.Board.Grid[r, c].HasMine)
                                {
                                    count++;
                                }
                            }
                        }
                        game.Board.Grid[row, col].AdjacentMines = count;
                    }
                }
            }
            
            // Act - Reveal all cells except the mine
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    if (game.Board.Grid[row, col].HasMine)
                    {
                        continue; // Skip the mine
                    }
                    
                    game.RevealCell(row, col);
                }
            }
            
            // Assert
            Assert.True(game.IsGameOver, "Game should be over");
            Assert.True(game.IsGameWon, "Game should be won");
            Assert.False(game.Board.IsGameLost, "Game should not be lost");
        }
        
        [Fact]
        public void Game_RevealingMine_ResultsInLoss()
        {
            // Arrange
            Game game = new Game(3, 1);
            
            // Force mine placement
            game.RevealCell(0, 0); // Trigger the first move to place mines
            
            // Reset the board state - keep mines but clear revealed cells
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    game.Board.Grid[row, col].IsRevealed = false;
                }
            }
            
            // Place a mine manually in a specific location
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    game.Board.Grid[row, col].HasMine = false;
                }
            }
            
            // Place a single mine at position 1,1
            game.Board.Grid[1, 1].HasMine = true;
            
            // Act - Reveal the mine
            bool isGameOver = game.RevealCell(1, 1);
            
            // Assert
            Assert.True(isGameOver, "Game should be over");
            Assert.True(game.Board.IsGameLost, "Game should be lost");
            Assert.False(game.IsGameWon, "Game should not be won");
        }
        
        [Fact]
        public void Game_FlaggingCell_PreventsReveal()
        {
            // Arrange
            Game game = new Game(3, 1);
            bool cellWasRevealed = game.Board.Grid[0, 0].IsRevealed;
            
            // Act
            game.ToggleFlag(0, 0);
            bool result = game.RevealCell(0, 0);
            
            // Assert
            Assert.False(result, "Flagged cell should not be revealed");
            Assert.Equal(cellWasRevealed, game.Board.Grid[0, 0].IsRevealed);
            Assert.True(game.Board.Grid[0, 0].IsFlagged, "Cell should remain flagged");
        }
        
        [Fact]
        public void Game_ToggleFlag_SwitchesFlagState()
        {
            // Arrange
            Game game = new Game(3, 1);
            
            // Act & Assert - First toggle should set flag
            bool result1 = game.ToggleFlag(0, 0);
            Assert.True(result1, "Toggle flag should return true");
            Assert.True(game.Board.Grid[0, 0].IsFlagged, "Cell should be flagged");
            
            // Act & Assert - Second toggle should remove flag
            bool result2 = game.ToggleFlag(0, 0);
            Assert.True(result2, "Toggle flag should return true");
            Assert.False(game.Board.Grid[0, 0].IsFlagged, "Cell should not be flagged");
        }
    }
} 