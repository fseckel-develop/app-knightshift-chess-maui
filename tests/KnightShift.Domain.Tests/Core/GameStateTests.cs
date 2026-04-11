using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Tests.Core;

public class GameStateTests
{
    [Fact]
    public void Clone_Should_Create_Independent_Copy()
    {
        var game = new GameState();

        var pos = new Position('e', 2);
        game.Board.SetPiece(pos, new Piece(PieceType.Pawn, PieceColor.White));

        var clone = game.Clone();

        clone.Board.MovePiece(pos, new Position('e', 4));

        // original unchanged
        Assert.NotNull(game.Board.GetPiece(pos));
        Assert.Null(game.Board.GetPiece(new Position('e', 4)));
    }

    [Fact]
    public void ApplyMove_Should_Update_Board_And_Switch_Turn()
    {
        var game = new GameState();

        var from = new Position('e', 2);
        var to = new Position('e', 4);

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

        var from = new Position('e', 2);
        var to = new Position('e', 4);

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
