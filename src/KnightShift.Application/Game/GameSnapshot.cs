using KnightShift.Domain.Core;

namespace KnightShift.Application.Game;

public sealed record GameSnapshot(
    GameState State, 
    Move? Move
);
