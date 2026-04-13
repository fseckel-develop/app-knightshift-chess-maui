using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class BishopMoveGeneratorTests
{
    private readonly BishopMoveGenerator _generator = new();

    [Fact]
    public void Bishop_Should_Move_Diagonally()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Bishop, PieceColor.White, "d4")
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Bishop, PieceColor.White),
            Position.CreateFromAlgebraic("d4"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("h8"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("g1"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("a1"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("a7"));
    }
}
