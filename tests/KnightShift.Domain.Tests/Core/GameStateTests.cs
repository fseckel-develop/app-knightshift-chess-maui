using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Tests.Core;

public class GameStateTests
{
    [Fact]
    public void Clone_Should_Create_Independent_Copy()
    {
        var game = new GameState();

        var position = Position.CreateFromAlgebraic("e2");
        game.Board.SetPiece(position, new Piece(PieceType.Pawn, PieceColor.White));

        var clone = game.Clone();

        clone.Board.MovePiece(position, Position.CreateFromAlgebraic("e4"));

        // original unchanged
        Assert.NotNull(game.Board.GetPiece(position));
        Assert.Null(game.Board.GetPiece(Position.CreateFromAlgebraic("e4")));
    }

    [Fact]
    public void ApplyMove_Should_Update_Board_And_Switch_Turn()
    {
        var game = new GameState();

        var from = Position.CreateFromAlgebraic("e2");
        var to = Position.CreateFromAlgebraic("e4");

        game.Board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

        var newState = game.ApplyMove(new Move(from, to));

        Assert.Null(newState.Board.GetPiece(from));
        Assert.NotNull(newState.Board.GetPiece(to));
        Assert.Equal(PieceColor.Black, newState.CurrentTurn);
    }

    [Fact]
    public void ApplyMove_Should_Not_Modify_Original_State()
    {
        var game = new GameState();

        var from = Position.CreateFromAlgebraic("e2");
        var to = Position.CreateFromAlgebraic("e4");

        game.Board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

        var newState = game.ApplyMove(new Move(from, to));

        // original state unchanged
        Assert.NotNull(game.Board.GetPiece(from));
        Assert.Null(game.Board.GetPiece(to));

        // new state updated
        Assert.Null(newState.Board.GetPiece(from));
        Assert.NotNull(newState.Board.GetPiece(to));
    }
}
