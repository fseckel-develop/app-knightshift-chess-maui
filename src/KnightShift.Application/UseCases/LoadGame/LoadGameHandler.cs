using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.LoadGame;

public class LoadGameHandler
{
    private readonly IGameService _game;

    public LoadGameHandler(IGameService game)
    {
        _game = game;
    }

    public void Handle(LoadGameCommand command)
    {
        _game.LoadGame(command.Pgn);
    }
}
