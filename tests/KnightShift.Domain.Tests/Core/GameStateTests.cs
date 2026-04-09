using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Tests.Core;

public class GameStateTests
{
    [Fact]
    public void ApplyMove_Should_Update_Board_And_Switch_Turn()
    {
        var game = new GameState();

        var from = new Position('e', 2);
        var to = new Position('e', 4);

        game.Board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

        game.ApplyMove(new Move(from, to));

        Assert.Null(game.Board.GetPiece(from));
        Assert.NotNull(game.Board.GetPiece(to));
        Assert.Equal(PieceColor.Black, game.CurrentTurn);
    }
}
