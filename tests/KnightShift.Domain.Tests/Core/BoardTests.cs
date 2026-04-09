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
        var position = new Position('e', 4);
        var piece = new Piece(PieceType.Knight, PieceColor.White);

        board.SetPiece(position, piece);

        var result = board.GetPiece(position);

        Assert.Equal(piece, result);
    }

    [Fact]
    public void GetPiece_On_Empty_Square_Should_Return_Null()
    {
        var board = new Board();
        var position = new Position('d', 4);

        var result = board.GetPiece(position);

        Assert.Null(result);
    }

    [Fact]
    public void MovePiece_Should_Move_Piece_And_Clear_Source()
    {
        var board = new Board();
        var from = new Position('e', 2);
        var to = new Position('e', 4);
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
        var from = new Position('a', 1);
        var to = new Position('a', 2);

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

        var p1 = new Position('a', 1);
        var p2 = new Position('h', 8);

        board.SetPiece(p1, new Piece(PieceType.Rook, PieceColor.White));
        board.SetPiece(p2, new Piece(PieceType.King, PieceColor.Black));

        var pieces = board.GetAllPieces().ToList();

        Assert.Equal(2, pieces.Count);
        Assert.Contains(pieces, p => p.position == p1);
        Assert.Contains(pieces, p => p.position == p2);
    }
}
