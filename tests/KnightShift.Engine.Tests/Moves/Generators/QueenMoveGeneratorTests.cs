using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class QueenMoveGeneratorTests
{
    private readonly QueenMoveGenerator _generator = new();

    [Fact]
    public void Queen_Should_Move_In_All_Directions()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Queen, PieceColor.White, "d4")
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Queen, PieceColor.White),
            Position.CreateFromAlgebraic("d4"));

        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("d1"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("d8"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("a4"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("h4"));
        
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("h8"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("g1"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("a1"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("a7"));
    }
}
