using System;
using System.IO;
using Xunit;
using Minesweeper.UI;
using Minesweeper.Models;

namespace Minesweeper.Tests.UI
{
    public class OutputFormatterTests
    {
        [Fact]
        public void DisplayGameOverMessage_Win_ShowsWinMessage()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);
            
            // Act
            OutputFormatter.DisplayGameOverMessage(true);
            
            // Assert
            string result = output.ToString();
            Assert.Contains("Congratulations", result);
            Assert.Contains("won", result);
        }
        
        [Fact]
        public void DisplayGameOverMessage_Loss_ShowsLossMessage()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);
            
            // Act
            OutputFormatter.DisplayGameOverMessage(false);
            
            // Assert
            string result = output.ToString();
            Assert.Contains("detonated", result);
            Assert.Contains("Game over", result);
        }
        
        [Fact]
        public void DisplayCellInfo_WithAdjacentMines_ShowsCorrectCount()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);
            var cell = new Cell(0, 0);
            cell.AdjacentMines = 3;
            
            // Act
            OutputFormatter.DisplayCellInfo(cell);
            
            // Assert
            string result = output.ToString();
            Assert.Contains("3 adjacent mines", result);
        }
        
        [Fact]
        public void DisplayCellInfo_WithMinedCell_DisplaysNothing()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);
            var cell = new Cell(0, 0) { HasMine = true };
            
            // Act
            OutputFormatter.DisplayCellInfo(cell);
            
            // Assert
            string result = output.ToString();
            Assert.Equal("", result);
        }
        
        [Fact]
        public void DisplayWelcomeMessage_ShowsWelcomeText()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);
            
            // Act
            OutputFormatter.DisplayWelcomeMessage();
            
            // Assert
            string result = output.ToString();
            Assert.Contains("Welcome to Minesweeper", result);
        }
    }
} 