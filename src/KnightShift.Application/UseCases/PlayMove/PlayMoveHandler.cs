using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.PlayMove;

public class PlayMoveHandler
{
    private readonly IGameService _game;

    public PlayMoveHandler(IGameService game)
    {
        _game = game;
    }

    public void Handle(PlayMoveCommand command)
    {
        _game.ApplyMove(command.Move);
    }
}
