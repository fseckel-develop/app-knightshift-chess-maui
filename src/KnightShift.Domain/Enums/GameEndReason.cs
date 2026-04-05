namespace KnightShift.Domain.Enums;

public enum GameEndReason
{
    None,
    Checkmate,
    Stalemate,
    Resignation,
    Abandonment,
    DrawAgreement,
    FiftyMoveRule,
    ThreefoldRepetition,
    InsufficientMaterial
}
