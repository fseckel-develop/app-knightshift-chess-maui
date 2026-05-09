using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Application.UseCases.UndoMove;

public class UndoMoveHandler
{
    private readonly IGameService _game;

    public UndoMoveHandler(IGameService game)
    {
        _game = game;
    }

    public MoveDto? Handle(UndoMoveCommand _)
    {
        var currentState = _game.GetState();
        
        _game.UndoMove();

        return currentState.LastMove;
    }
}
