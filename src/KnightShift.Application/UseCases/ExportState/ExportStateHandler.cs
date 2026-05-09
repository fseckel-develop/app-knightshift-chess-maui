using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.ExportState;

public class ExportStateHandler
{
    private readonly IGameService _game;

    public ExportStateHandler(IGameService game)
    {
        _game = game;
    }

    public string Handle(ExportStateQuery _)
    {
        return _game.ExportState();
    }
}
