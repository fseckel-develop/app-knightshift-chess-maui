using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class RookMoveGeneratorTests
{
    private readonly RookMoveGenerator _generator = new();

    [Fact]
    public void Rook_Should_Move_In_Straight_Lines()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d4")
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d4"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d1"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d8"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("a4"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("h4"));
    }
}
