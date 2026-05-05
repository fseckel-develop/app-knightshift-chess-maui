using KnightShift.Domain.Core;

namespace KnightShift.Application.Game.Models;

public sealed record GameRecord
(
    GameState InitialState,
    IReadOnlyList<Move> Moves
);
