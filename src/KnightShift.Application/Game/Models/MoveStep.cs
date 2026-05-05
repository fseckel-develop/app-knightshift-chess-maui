using KnightShift.Domain.Core;

namespace KnightShift.Application.Game.Models;

public sealed record MoveStep
(
    Move Move,
    GameState StateBeforeMove,
    GameState StateAfterMove
);
