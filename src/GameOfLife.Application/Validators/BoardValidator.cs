using GameOfLife.Domain;

namespace GameOfLife.Application.Validators
{
    public static class BoardValidator
    {
        public static void ValidateBoardSize(bool[,] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            if (rows > Constants.MaxBoardRows || cols > Constants.MaxBoardColumns)
            {
                throw new ArgumentException($"Board size exceeds maximum allowed dimensions ({Constants.MaxBoardRows}x{Constants.MaxBoardColumns}).");
            }

            if (rows < 1 || cols < 1)
            {
                throw new ArgumentException("Board dimensions must be at least 1x1.");
            }
        }
    }
}