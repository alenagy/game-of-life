using GameOfLife.Application.Interfaces;
using GameOfLife.Application.Validators;
using GameOfLife.Domain.Entities;

namespace GameOfLife.Application.Services
{
    public class BoardService
    {
        private readonly IBoardRepository _repository;

        public BoardService(IBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateBoardAsync(int rows, int cols, bool[,] initialState)
        {
            BoardValidator.ValidateBoardSize(initialState);            

            if (initialState.GetLength(0) != rows || initialState.GetLength(1) != cols)
            {
                throw new ArgumentException("Initial state must match the board dimensions.");
            }

            var board = new Board(rows, cols);
            board.LiveCells = GetLiveCellsFromBoard(initialState);
            board = await _repository.AddBoardAsync(board);
            
            return board.Id;
        }

        public async Task<Board?> GetBoardAsync(Guid id) => await _repository.GetBoardAsync(id);

        private List<CellCoordinate> GetLiveCellsFromBoard(bool[,] initialState)
        {
            List<CellCoordinate> liveCells = new List<CellCoordinate>();

            for(int i = 0; i < initialState.GetLength(0); i++)
            {
                for(int j = 0; j < initialState.GetLength(1); j++)
                {
                    if (initialState[i,j])
                    {
                        liveCells.Add(new CellCoordinate() { Row = i, Col = j });
                    }
                }
            }

            return liveCells;
        }
    }
}