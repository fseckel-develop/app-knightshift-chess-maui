using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Application.UseCases.GetState;

public class GetStateHandler
{
    private readonly IGameService _game;

    public GetStateHandler(IGameService game)
    {
        _game = game;
    }

    public GameStateDto Handle(GetStateQuery _)
    {
        return _game.GetState();
    }
}
