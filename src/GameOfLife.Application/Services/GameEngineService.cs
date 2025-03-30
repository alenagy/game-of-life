using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Utilities;

namespace GameOfLife.Application.Services
{
    public static class GameEngineService
    {
        public static HashSet<CellCoordinate> GetNextState(HashSet<CellCoordinate> liveCells, int rows, int cols)
        {
            HashSet<CellCoordinate> nextState = new HashSet<CellCoordinate>();
            HashSet<CellCoordinate> candidates = new HashSet<CellCoordinate>();

            // Add the 8 neighbouring cells and the live one, to evaluate only potentially changing
            // sections of the board.
            foreach(var cell in liveCells)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int newRow = cell.Row + dr;
                        int newCol = cell.Col + dc;
                        // Ensure candidate is within board boundaries.
                        if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
                        {
                            candidates.Add(new CellCoordinate() { Row = newRow, Col = newCol});
                        }
                    }
                }
            }

            foreach(var candidate in candidates)
            {
                int liveNeighbors = CountLiveNeighbors(candidate, liveCells);
                bool isAlive = liveCells.Contains(candidate);

                // Apply Game of Life rules.
                if (isAlive && (liveNeighbors == 2 || liveNeighbors == 3))
                {
                    nextState.Add(candidate);
                }
                else if (!isAlive && liveNeighbors == 3)
                {
                    nextState.Add(candidate);
                }
            }
            return nextState;
        }

        public static HashSet<CellCoordinate> GetStateAfterGenerations(HashSet<CellCoordinate> liveCells, int rows, int cols, int generations)
        {
            HashSet<CellCoordinate> state = liveCells;
            for (int i = 0; i < generations; i++)
            {
                state = GetNextState(state, rows, cols);
            }

            return state;
        }

        public static (HashSet<CellCoordinate> finalState, bool isStable, bool maxGenerationsExceeded) GetFinalState(HashSet<CellCoordinate> liveCells, int rows, int cols, int maxGenerations)
        {
            HashSet<CellCoordinate> state = liveCells;
            HashSet<string> seenStates = new HashSet<string>();

            for (int i = 0; i < maxGenerations; i++)
            {
                string serializedState = SerializeBoard(state);
                if (seenStates.Contains(serializedState))
                {
                    // Board has stabilized (loop detected)
                    return (state, true, false);
                }

                seenStates.Add(serializedState);
                HashSet<CellCoordinate> nextState = GetNextState(state, rows, cols);

                if (state.SetEquals(nextState))
                {
                    // Board has stabilized (no change)
                    return (nextState, true, false);
                }

                // Update the state for the next iteration.
                state = nextState;
            }

            // After iterating maxGenerations without stabilization
            return (state, false, true);
        }

        private static int CountLiveNeighbors(CellCoordinate candidate, HashSet<CellCoordinate> liveCells)
        {
            int count = 0;
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                    {
                        continue;
                    }

                    if (liveCells.Contains(new CellCoordinate() { Row = candidate.Row + dr, Col = candidate.Col + dc}))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static string SerializeBoard(HashSet<CellCoordinate> state)
        {
            // Sort the coordinates to generate a consistent representation.
            var sorted = state.OrderBy(cell => cell.Row).ThenBy(cell => cell.Col);
            return string.Join(";", sorted.Select(cell => $"{cell.Row},{cell.Col}"));
        }
    }
}