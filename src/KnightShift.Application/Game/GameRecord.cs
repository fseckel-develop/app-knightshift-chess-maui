using KnightShift.Domain.Core;

namespace KnightShift.Application.Game;

public sealed record GameRecord(
    GameState InitialState,
    IReadOnlyList<Move> Moves
);
