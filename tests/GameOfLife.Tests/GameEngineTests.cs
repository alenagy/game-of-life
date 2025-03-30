using System;
using Xunit;
using GameOfLife.Application.Services;
using GameOfLife.Domain;
using GameOfLife.Domain.Entities;

namespace GameOfLife.Tests
{
    public class GameEngineServiceTests
    {
        [Fact]
        public void GetNextState_ShouldReturnExpectedResult_ForBlinker()
        {
            // Arrange: Create a 5x5 board with a horizontal "blinker" in the middle.
            HashSet<CellCoordinate> liveCells = new HashSet<CellCoordinate>()
            {
                new CellCoordinate() { Row = 2, Col = 1},
                new CellCoordinate() { Row = 2, Col = 2},
                new CellCoordinate() { Row = 2, Col = 3}
            };

            // Act: Compute next state.
            var nextState = GameEngineService.GetNextState(liveCells, 5, 5);

            // Assert: Expect a vertical blinker at positions (1,2), (2,2), (3,2).
            Assert.DoesNotContain(new CellCoordinate() { Row = 2, Col = 1 }, nextState);
            Assert.Contains(new CellCoordinate() { Row = 1, Col = 2 }, nextState);
            Assert.Contains(new CellCoordinate() { Row = 2, Col = 2 }, nextState);
            Assert.Contains(new CellCoordinate() { Row = 3, Col = 2 }, nextState);
            Assert.DoesNotContain(new CellCoordinate() { Row = 2, Col = 3 }, nextState);
        }

        [Fact]
        public void GetStateAfterGenerations_ShouldReturnSameState_ForStillLife()
        {
            // Arrange: Create a 4x4 board with a stable 2x2 block (still life).
            HashSet<CellCoordinate> liveCells = new HashSet<CellCoordinate>()
            {
                new CellCoordinate() { Row = 1, Col = 1},
                new CellCoordinate() { Row = 1, Col = 2},
                new CellCoordinate() { Row = 2, Col = 1},
                new CellCoordinate() { Row = 2, Col = 2}
            };

            // Act: Advance 5 generations.
            var futureState = GameEngineService.GetStateAfterGenerations(liveCells, 4, 4, 5);

            // Assert: The state should remain unchanged.
            Assert.True(liveCells.SetEquals(futureState));
        }

        [Fact]
        public void GetFinalState_ShouldDetectStableState()
        {
            // Arrange: Using the same stable block from above.
            HashSet<CellCoordinate> liveCells = new HashSet<CellCoordinate>()
            {
                new CellCoordinate() { Row = 1, Col = 1},
                new CellCoordinate() { Row = 1, Col = 2},
                new CellCoordinate() { Row = 2, Col = 1},
                new CellCoordinate() { Row = 2, Col = 2}
            };

            // Act: Check for a final state within 10 generations.
            var (finalState, isStable, maxGenerationsExceeded) = GameEngineService.GetFinalState(liveCells, 4, 4, 10);

            // Assert: The board should be stable and equal to the input block.
            Assert.True(isStable);
            Assert.True(liveCells.SetEquals(finalState));
        }

        [Fact]
        public void GetFinalState_ShouldStopAfterMaxGenerations()
        {
            // Place the r-pentomino near the center.
            // r-pentomino pattern (relative coordinates):
            // (0,1), (0,2), (1,0), (1,1), (2,1)
            HashSet<CellCoordinate> liveCells = new HashSet<CellCoordinate>()
            {
                new CellCoordinate() { Row = 50, Col = 51},
                new CellCoordinate() { Row = 50, Col = 52},
                new CellCoordinate() { Row = 51, Col = 50},
                new CellCoordinate() { Row = 51, Col = 51},
                new CellCoordinate() { Row = 52, Col = 51}
            };


            // Act: Check for a final state within max generations + 1.
            var (finalState, isStable, maxGenerationsExceeded) = GameEngineService.GetFinalState(liveCells, 100, 100, Constants.MaxGenerationsAllowed + 1);

            // Assert: The board shouldn't be stable and maxGenerationExceeded should be true.
            Assert.False(isStable);
            Assert.True(maxGenerationsExceeded);
        }

        [Fact]
        public void LargeBoard_WithStableBlock_DoesNotCrashAndIsStableWithin99Generations()
        {
            // Arrange: Create a 1000x1000 board (all cells dead by default)
            // Insert a stable 2x2 block in the middle of the board.
            // The 2x2 block is a still life that never changes.
            HashSet<CellCoordinate> liveCells = new HashSet<CellCoordinate>()
            {
                new CellCoordinate() { Row = 500, Col = 500},
                new CellCoordinate() { Row = 500, Col = 501},
                new CellCoordinate() { Row = 501, Col = 500},
                new CellCoordinate() { Row = 501, Col = 501}
            };

            // Act: Run the game logic for up to 99 generations to detect a final stable state.
            var (finalState, isStable, maxGenerationExceeded) = GameEngineService.GetFinalState(liveCells, 1000, 1000, 99);

            // Assert: The board should be stable.
            Assert.True(isStable);

            // And the stable block should remain unchanged.
            Assert.True(liveCells.SetEquals(finalState));
        }
    }
}