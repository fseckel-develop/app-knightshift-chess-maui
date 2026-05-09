using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.ExportGame;

public class ExportGameHandler
{
    private readonly IGameService _game;

    public ExportGameHandler(IGameService game)
    {
        _game = game;
    }

    public string Handle(ExportGameQuery _)
    {
        return _game.ExportGame();
    }
}
