# Minesweeper Game

A command-line implementation of the classic Minesweeper game in C#.

## Requirements

- .NET 8.0 or higher
- Windows, Linux, or macOS

## How to Play

1. Run the application
2. Enter the size of the grid (e.g., 4 for a 4x4 grid)
3. Enter the number of mines to place on the grid (maximum is 35% of the total squares)
4. Select squares to reveal by entering coordinates (e.g., A1)
5. If a square has no adjacent mines, it will automatically reveal adjacent squares
6. Win by revealing all non-mine squares
7. Lose by revealing a square with a mine

## Project Structure

- `Program.cs` - Main entry point for the application
- `Game.cs` - Game logic and flow
- `Board.cs` - Board representation and operations
- `Cell.cs` - Individual cell representation
- `InputHandler.cs` - User input processing
- `OutputFormatter.cs` - Game output formatting

## Implementation Plan

### Core Features
1. Create a board with user-specified dimensions
2. Randomly place mines on the board
3. Calculate adjacent mine counts for each cell
4. Allow users to select squares to reveal
5. Auto-reveal adjacent squares with zero adjacent mines
6. Detect win/lose conditions
7. Display the game board after each move

### Technical Approach
- Use object-oriented design with SOLID principles
- Implement TDD approach with comprehensive unit tests
- Create a clear separation of concerns between game logic and user interface
- Handle edge cases and input validation

## Tasks Breakdown

1. **Setup Project Structure**
   - Create solution and project files
   - Set up test project
   - Configure build settings

2. **Implement Core Models**
   - Create Cell class with properties (HasMine, IsRevealed, AdjacentMines)
   - Implement Board class with grid generation and mine placement
   - Build Game class to manage game state and flow

3. **Implement Game Logic**
   - Develop algorithm to calculate adjacent mines
   - Create logic for revealing cells
   - Implement recursive revealing of adjacent empty cells
   - Add win/lose condition detection

4. **User Interface**
   - Create input handling for user commands
   - Implement board visualization
   - Build game loop with user interaction
   - Add game status messages

5. **Testing**
   - Unit tests for Cell class implemented
   - Unit tests for Board class implemented
   - Unit tests for Game class implemented
   - UI component tests added for OutputFormatter
   - Integration tests implemented for game flow scenarios
   - Edge cases and input validation tested

6. **Refinement**
   - Enhance user experience
   - Optimize performance
   - Refactor code for readability and maintainability
   - Add documentation

## Running the Application

```
dotnet run
```

## Running Tests

```
dotnet test
```

## Completed Tasks

All the tasks listed in the implementation plan have been completed:

1. **Project Structure**
   - Solution and project files created
   - Test project set up with xUnit
   - Build settings configured

2. **Core Models**
   - Cell class implemented with all required properties
   - Board class implemented with grid generation and mine placement
   - Game class implemented to manage game state and flow

3. **Game Logic**
   - Algorithm for calculating adjacent mines implemented
   - Cell revealing logic implemented
   - Recursive revealing of adjacent empty cells implemented
   - Win/lose condition detection implemented

4. **User Interface**
   - Input handling for user commands implemented
   - Board visualization implemented with clear formatting
   - Game loop with user interaction implemented
   - Status messages for game state implemented

5. **Testing**
   - Unit tests for Cell class implemented
   - Unit tests for Board class implemented
   - Unit tests for Game class implemented
   - UI component tests added for OutputFormatter
   - Integration tests implemented for game flow scenarios
   - Edge cases and input validation tested

6. **Refinement**
   - Code documentation added with XML comments
   - Input validation improved
   - Null handling added
   - Code organization improved

## Potential Improvements

While the current implementation satisfies all requirements, here are some potential improvements that could be made:

1. **UI Enhancements**
   - Add color support for better visualization (mines in red, numbers in different colors)
   - Implement a more graphical interface (using a library like Terminal.Gui)
   - Add a timer to track game duration

2. **Gameplay Features**
   - Add difficulty levels (easy, medium, hard) with preset board sizes and mine counts
   - Implement flagging functionality to mark potential mines
   - Add a first-move protection that guarantees the first click reveals a zero
   - Add a hint system that can help players when stuck

3. **Architecture Improvements**
   - Implement the Command pattern for player actions
   - Implement the Observer pattern for game state changes
   - Add a proper state machine for game states

4. **Technical Improvements**
   - Add logging
   - Implement save/load game functionality
   - Improve performance for larger board sizes
   - Add more comprehensive exception handling

5. **Testing Improvements**
   - Implement dependency injection for InputHandler to improve testability
   - Add more edge case tests
   - Implement property-based testing
   - Add automated UI tests that simulate actual game play
   - Add coverage reporting 