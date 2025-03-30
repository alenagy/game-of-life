using System;
using GameOfLife.Domain.Entities;

namespace GameOfLife.Application.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetBoardAsync(Guid id);
    Task<Board> AddBoardAsync(Board board);
}
