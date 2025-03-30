using System;
using GameOfLife.Domain.Entities;

namespace GameOfLife.Domain.Utilities;

public static class BoardExtensions
{
    public static bool[,] ToBoardState(this List<CellCoordinate> cellCoordinates, int rows, int cols)
    {
        bool[,] state = new bool[rows, cols];

        foreach(var coord in cellCoordinates)
        {
            state[coord.Row,coord.Col] = true;
        }

        return state;
    }

    public static bool[,] ToBoardState(this HashSet<CellCoordinate> cellCoordinates, int rows, int cols)
    {
        bool[,] state = new bool[rows, cols];

        foreach(var coord in cellCoordinates)
        {
            state[coord.Row,coord.Col] = true;
        }

        return state;
    }
}
