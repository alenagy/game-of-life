using GameOfLife.Domain.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;

namespace GameOfLife.Domain.Entities;

public class Board
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; } = Guid.NewGuid();

    public int Rows { get; private set; }
    public int Cols { get; private set; }
    // We store only live cells to optimize space and loading. We can generate a board
    // with the RowsxCols properties and then set the live cells on it. Unless the
    // board is highly dense, it would avoid saving a lot of dead cells that have no
    // inference in the board state change (no live cell neighbours)
    public List<CellCoordinate> LiveCells { get; set; } = new List<CellCoordinate>();

    public Board(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Board dimensions must be positive.");

        if (rows > Constants.MaxBoardRows || cols > Constants.MaxBoardColumns)
            throw new ArgumentException($"Board dimensions must be less than the maximum allowed of {Constants.MaxBoardRows}x{Constants.MaxBoardColumns}.");

        Rows = rows;
        Cols = cols;
    }
}