namespace KnightShift.Application.Contracts.DTOs;

public enum GameEndReasonDto
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
