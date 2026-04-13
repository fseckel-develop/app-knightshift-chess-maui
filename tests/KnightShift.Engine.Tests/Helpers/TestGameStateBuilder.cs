using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Engine.Tests.Helpers;

public class TestGameStateBuilder
{
    private readonly GameState _state = new();

    public TestGameStateBuilder WithPiece(PieceType type, PieceColor color, string square)
    {
        var pos = Position.CreateFromAlgebraic(square);
        _state.Board.SetPiece(pos, new Piece(type, color));
        return this;
    }

    public TestGameStateBuilder WithTurn(PieceColor color)
    {
        _state.CurrentTurn = color;
        return this;
    }


    public TestGameStateBuilder WithCastlingRights(
        bool whiteKingSide = false,
        bool whiteQueenSide = false,
        bool blackKingSide = false,
        bool blackQueenSide = false)
    {
        _state.WhiteCanCastleKingSide = whiteKingSide;
        _state.WhiteCanCastleQueenSide = whiteQueenSide;
        _state.BlackCanCastleKingSide = blackKingSide;
        _state.BlackCanCastleQueenSide = blackQueenSide;
        return this;
    }

    public TestGameStateBuilder WithWhiteCastling(bool kingSide = true, bool queenSide = true)
    {
        _state.WhiteCanCastleKingSide = kingSide;
        _state.WhiteCanCastleQueenSide = queenSide;
        return this;
    }

    public TestGameStateBuilder WithBlackCastling(bool kingSide = true, bool queenSide = true)
    {
        _state.BlackCanCastleKingSide = kingSide;
        _state.BlackCanCastleQueenSide = queenSide;
        return this;
    }

    public TestGameStateBuilder WithEnPassant(string square)
    {
        _state.EnPassantTarget = Position.CreateFromAlgebraic(square);
        return this;
    }

    public GameState Build() => _state;
}
