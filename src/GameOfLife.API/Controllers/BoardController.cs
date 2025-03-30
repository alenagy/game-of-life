using GameOfLife.API.Dto;
using GameOfLife.Application.Services;
using GameOfLife.Domain;
using GameOfLife.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class BoardController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        // The wording on the first point suggests to me its a new board, and not an update.
        // If there was indeed a need for an update of a board, this would become an Upsert
        // operation. If no id provided on the DTO, new board. Otherwise replace the state
        // with the specified state on the DTO. Probably in a separate endpoint.
        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] BoardDTO request)
        {
            var id = await _boardService.CreateBoardAsync(request.Rows, request.Cols, request.State);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            var board = await _boardService.GetBoardAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            return Ok(new BoardDTO() { Id = board.Id, Cols = board.Cols, Rows = board.Rows, State = board.LiveCells.ToBoardState(board.Rows, board.Cols) });
        }

        // Since it mentions getting the next state, and GET operations shouldn't modify the data,
        // I'm not persisting the next state on the given board. If that was a requirement I would
        // probably suggest a PUT endpoint, something like UpdateNextBoardState(id) where I would
        // return the next state, but also persist it on the board.
        // Same with the other methods for getting a state after X steps or the final state.
        [HttpGet("{id}/next")]
        public async Task<IActionResult> GetNextState(Guid id)
        {
            var board = await _boardService.GetBoardAsync(id);
            if (board == null) return NotFound();

            var nextState = GameEngineService.GetNextState(board.LiveCells.ToHashSet(), board.Rows, board.Cols);
            return Ok(nextState.ToBoardState(board.Rows, board.Cols));
        }
        
        [HttpGet("{id}/generations")]
        public async Task<IActionResult> GetStateAfterGenerations(Guid id, int generations)
        {
            var board = await _boardService.GetBoardAsync(id);
            if (board == null) return NotFound();

            var state = GameEngineService.GetStateAfterGenerations(board.LiveCells.ToHashSet(), board.Rows, board.Cols, Math.Min(generations, Constants.MaxGenerationsAllowed));
            return Ok(state);
        }

        [HttpGet("{id}/final")]
        public async Task<IActionResult> GetFinalState(Guid id, int maxGenerations)
        {
            var board = await _boardService.GetBoardAsync(id);
            if (board == null) return NotFound();

            var (finalState, isStable, maxGenerationsExceeded) = GameEngineService.GetFinalState(board.LiveCells.ToHashSet(), board.Rows, board.Cols, maxGenerations);
            if (maxGenerationsExceeded)
            {
                return BadRequest($"Exceeded the maximum number of generations allowed ({Constants.MaxGenerationsAllowed}) without a final state.");
            }

            if (!isStable)
            {
                return BadRequest($"Board was not stable after {maxGenerations} steps. You may try more steps with a limit of {Constants.MaxGenerationsAllowed}.");
            }

            return Ok(new { FinalState = finalState, IsStable = isStable });
        }
    }
}