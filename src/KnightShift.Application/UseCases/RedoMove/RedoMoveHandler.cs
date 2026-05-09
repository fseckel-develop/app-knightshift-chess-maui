using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.RedoMove;

public class RedoMoveHandler : IQueryHandler<RedoMoveCommand, MoveDto?>
{
    private readonly IGameService _game;

    public RedoMoveHandler(IGameService game)
    {
        _game = game;
    }

    public MoveDto? Handle(RedoMoveCommand _)
    {
        _game.RedoMove();
        
        return _game.GetState().LastMove;
    }
}
