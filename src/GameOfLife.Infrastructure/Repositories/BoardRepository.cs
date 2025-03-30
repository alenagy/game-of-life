using GameOfLife.Application.Interfaces;
using GameOfLife.Domain.Entities;
using GameOfLife.Infrastructure.Persistence;
using MongoDB.Driver;

namespace GameOfLife.Infrastructure.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IMongoCollection<Board> _boards;

        public BoardRepository(MongoDbContext context)
        {
            _boards = context.Boards;
        }

        public async Task<Board> AddBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            await _boards.InsertOneAsync(board);
            return board;
        }

        public async Task<Board?> GetBoardAsync(Guid id)
        {
            var board = await _boards.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (board == null)
            {
                throw new KeyNotFoundException($"Board with id {id} not found.");
            }
            return board;
        }
    }
}