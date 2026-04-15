using KnightShift.Application.Abstractions;
using KnightShift.Domain.Core;
using KnightShift.Infrastructure.Notation;

namespace KnightShift.Infrastructure.Factories;

public class FenGameStateFactory : IGameStateFactory
{
    private static readonly string StartPositionFen =
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";

    public GameState CreateInitialState()
        => FenParser.FromFen(StartPositionFen);
}
