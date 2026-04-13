using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Exceptions;

namespace KnightShift.Domain.Tests.Core;

public class BoardTests
{
    [Fact]
    public void SetPiece_Then_GetPiece_Should_Return_Same_Piece()
    {
        var board = new Board();
        var position = Position.CreateFromAlgebraic("e4");
        var piece = new Piece(PieceType.Knight, PieceColor.White);

        board.SetPiece(position, piece);

        var result = board.GetPiece(position);

        Assert.Equal(piece, result);
    }

    [Fact]
    public void GetPiece_On_Empty_Square_Should_Return_Null()
    {
        var board = new Board();
        var position = Position.CreateFromAlgebraic("d4");

        var result = board.GetPiece(position);

        Assert.Null(result);
    }

    [Fact]
    public void MovePiece_Should_Move_Piece_And_Clear_Source()
    {
        var board = new Board();
        var from = Position.CreateFromAlgebraic("e2");
        var to = Position.CreateFromAlgebraic("e4");
        var piece = new Piece(PieceType.Pawn, PieceColor.White);

        board.SetPiece(from, piece);

        board.MovePiece(from, to);

        Assert.Null(board.GetPiece(from));
        Assert.Equal(piece, board.GetPiece(to));
    }

    [Fact]
    public void MovePiece_Without_Piece_Should_Throw()
    {
        var board = new Board();
        var from = Position.CreateFromAlgebraic("a1");
        var to = Position.CreateFromAlgebraic("a2");

        Assert.Throws<PieceNotFoundException>(() => board.MovePiece(from, to));
    }

    [Fact]
    public void SetPiece_With_Invalid_Position_Should_Throw()
    {
        var board = new Board();
        var invalid = new Position('z', 99);
        var piece = new Piece(PieceType.Bishop, PieceColor.Black);

        Assert.Throws<InvalidPositionException>(() => board.SetPiece(invalid, piece));
    }

    [Fact]
    public void GetAllPieces_Should_Return_All_Placed_Pieces()
    {
        var board = new Board();

        var position1 = Position.CreateFromAlgebraic("a1");
        var position2 = Position.CreateFromAlgebraic("h8");

        board.SetPiece(position1, new Piece(PieceType.Rook, PieceColor.White));
        board.SetPiece(position2, new Piece(PieceType.King, PieceColor.Black));

        var pieces = board.GetAllPieces().ToList();

        Assert.Equal(2, pieces.Count);
        Assert.Contains(pieces, piece => piece.position == position1);
        Assert.Contains(pieces, piece => piece.position == position2);
    }

    [Fact]
    public void Clone_Should_Copy_All_Pieces()
    {
        var board = new Board();

        var position1 = Position.CreateFromAlgebraic("a1");
        var position2 = Position.CreateFromAlgebraic("h8");

        board.SetPiece(position1, new Piece(PieceType.Rook, PieceColor.White));
        board.SetPiece(position2, new Piece(PieceType.King, PieceColor.Black));

        var clone = board.Clone();

        Assert.Equal(board.GetPiece(position1), clone.GetPiece(position1));
        Assert.Equal(board.GetPiece(position2), clone.GetPiece(position2));
    }

    [Fact]
    public void Clone_Should_Create_Independent_Instance()
    {
        var board = new Board();

        var from = Position.CreateFromAlgebraic("e2");
        var to = Position.CreateFromAlgebraic("e4");

        board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

        var clone = board.Clone();

        clone.MovePiece(from, to);

        // original unchanged
        Assert.NotNull(board.GetPiece(from));
        Assert.Null(board.GetPiece(to));

        // clone updated
        Assert.Null(clone.GetPiece(from));
        Assert.NotNull(clone.GetPiece(to));
    }
}
