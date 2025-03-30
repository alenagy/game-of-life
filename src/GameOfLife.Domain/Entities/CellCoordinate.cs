using System;

namespace GameOfLife.Domain.Entities;

public class CellCoordinate
{
    public int Row { get; set; }
    public int Col { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not CellCoordinate other)
            return false;
        return Row == other.Row && Col == other.Col;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }
}
