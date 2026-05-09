using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game.Models;

namespace KnightShift.Application.UseCases.GetHistory;

public class GetHistoryHandler
{
    private readonly IGameService _game;

    public GetHistoryHandler(IGameService game)
    {
        _game = game;
    }

    public IEnumerable<MoveStep> Handle(GetHistoryQuery _)
    {
        return _game.GetHistory();
    }
}
