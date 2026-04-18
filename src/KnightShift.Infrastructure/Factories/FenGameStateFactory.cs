using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;

namespace KnightShift.Infrastructure.Factories;

public class FenGameStateFactory : IGameStateFactory
{
    private readonly IGameStateSerializer _serializer;

    private readonly string StartPositionFen =
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";

    public FenGameStateFactory(IGameStateSerializer serializer)
    {
        _serializer = serializer;
    }

    public GameState CreateInitialState()
        => _serializer.Deserialize(StartPositionFen);
}
