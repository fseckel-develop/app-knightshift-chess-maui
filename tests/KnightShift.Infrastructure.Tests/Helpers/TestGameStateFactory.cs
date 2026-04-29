using KnightShift.Infrastructure.Serialization;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Tests.Helpers;

public static class TestGameStateFactory
{
    private static readonly IGameStateSerializer _serializer = new FenGameStateSerializer();

    public static GameState CreateFromFen(string fen)
        => _serializer.Deserialize(fen);

    public static GameState CreateEmpty()
        => CreateFromFen("8/8/8/8/8/8/8/8 w - -");

    public static GameState CreateInitial()
        => CreateFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");

    public static GameState CreateEmptyWithKings()
    {
        var state = new GameState();

        state.Board.SetPiece(Position.CreateFromAlgebraic("e1"), new Piece(PieceType.King, PieceColor.White));
        state.Board.SetPiece(Position.CreateFromAlgebraic("e8"), new Piece(PieceType.King, PieceColor.Black));

        return state;
    }
}
