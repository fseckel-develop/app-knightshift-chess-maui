using FluentAssertions;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Evaluation;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Evaluation;

public class GameResultEvaluatorTests
{
    private readonly GameResultEvaluator _evaluator = EngineTestFactory.CreateEvaluator();

    [Fact]
    public void Should_Set_Game_As_Ongoing_When_Moves_Available()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.King, PieceColor.Black, "e8")
            .WithTurn(PieceColor.White)
            .Build();

        _evaluator.Evaluate(state);

        state.Result.Should().Be(GameResult.Ongoing);
        state.EndReason.Should().Be(GameEndReason.None);
    }

    [Fact]
    public void Should_Detect_Checkmate()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "h1")
            .WithPiece(PieceType.Queen, PieceColor.Black, "g2")
            .WithPiece(PieceType.Rook, PieceColor.Black, "h2")
            .WithTurn(PieceColor.White)
            .Build();

        _evaluator.Evaluate(state);

        state.Result.Should().Be(GameResult.BlackWins);
        state.EndReason.Should().Be(GameEndReason.Checkmate);
    }

    [Fact]
    public void Should_Detect_Stalemate()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "h1")
            .WithPiece(PieceType.King, PieceColor.Black, "f2")
            .WithPiece(PieceType.Queen, PieceColor.Black, "g3")
            .WithTurn(PieceColor.White)
            .Build();

        _evaluator.Evaluate(state);

        state.Result.Should().Be(GameResult.Draw);
        state.EndReason.Should().Be(GameEndReason.Stalemate);
    }

    [Fact]
    public void Should_Assign_Winner_Based_On_CurrentTurn()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.Black, "h8")
            .WithPiece(PieceType.Queen, PieceColor.White, "g7")
            .WithPiece(PieceType.Rook, PieceColor.White, "h7")
            .WithTurn(PieceColor.Black)
            .Build();

        _evaluator.Evaluate(state);

        state.Result.Should().Be(GameResult.WhiteWins);
        state.EndReason.Should().Be(GameEndReason.Checkmate);
    }
}
