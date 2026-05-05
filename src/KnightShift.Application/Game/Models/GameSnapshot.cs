using KnightShift.Domain.Core;

namespace KnightShift.Application.Game.Models;

public sealed record GameSnapshot
(
    GameState State, 
    Move? Move
);
