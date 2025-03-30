using System;
using System.Text.Json.Serialization;
using GameOfLife.Domain.Utilities;

namespace GameOfLife.API.Dto;

public class BoardDTO
{
    public Guid Id { get; set; }
    public int Rows { get; set; }
    public int Cols { get; set; }

    [JsonConverter(typeof(BoolMultiDimensionalArrayConverter))]
    public bool[,] State { get; set; } = new bool[0,0];
}
