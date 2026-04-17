using KnightShift.Application.Mappers;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Application.Tests.Mappers;

public class GameStateMapperTests
{
    [Fact]
    public void ToDto_ShouldCreate8x8Board()
    {
        var state = new GameState();

        var dto = GameStateMapper.ToDto(state);

        Assert.Equal(8, dto.Board.Length);
        Assert.All(dto.Board, row => Assert.Equal(8, row.Length));
    }

    [Fact]
    public void ToDto_ShouldMapEmptySquaresAsDot()
    {
        var state = new GameState();

        var dto = GameStateMapper.ToDto(state);

        Assert.All(dto.Board, row =>
            Assert.All(row, cell => Assert.Equal(".", cell))
        );
    }

    [Fact]
    public void ToDto_ShouldMapWhitePieceAsUppercase()
    {
        var state = new GameState();

        var position = Position.CreateFromAlgebraic("e4");
        state.Board.SetPiece(position, new Piece(PieceType.Queen, PieceColor.White));

        var dto = GameStateMapper.ToDto(state);

        var (row, col) = Position.ToCoords(position);

        Assert.Equal("Q", dto.Board[row][col]);
    }

    [Fact]
    public void ToDto_ShouldMapBlackPieceAsLowercase()
    {
        var state = new GameState();

        var position = Position.CreateFromAlgebraic("d5");
        state.Board.SetPiece(position, new Piece(PieceType.Knight, PieceColor.Black));

        var dto = GameStateMapper.ToDto(state);

        var (row, column) = Position.ToCoords(position);

        Assert.Equal("n", dto.Board[row][column]);
    }

    [Fact]
    public void ToDto_ShouldMapAllPieceTypesCorrectly()
    {
        var state = new GameState();

        var testCases = new[]
        {
            (PieceType.Pawn,   "p"),
            (PieceType.Knight, "n"),
            (PieceType.Bishop, "b"),
            (PieceType.Rook,   "r"),
            (PieceType.Queen,  "q"),
            (PieceType.King,   "k"),
        };

        int column = 0;

        foreach (var (type, symbol) in testCases)
        {
            var position = Position.CreateFromCoords(0, column);
            state.Board.SetPiece(position, new Piece(type, PieceColor.Black));
            column++;
        }

        var dto = GameStateMapper.ToDto(state);

        for (int i = 0; i < testCases.Length; i++)
        {
            Assert.Equal(testCases[i].Item2, dto.Board[0][i]);
        }
    }

    [Fact]
    public void ToDto_ShouldMapMetadataCorrectly()
    {
        var state = new GameState
        {
            CurrentTurn = PieceColor.Black,
            Result = GameResult.Draw,
            EndReason = GameEndReason.Stalemate
        };

        var dto = GameStateMapper.ToDto(state);

        Assert.Equal("Black", dto.CurrentTurn);
        Assert.Equal("Draw", dto.Result);
        Assert.Equal("Stalemate", dto.EndReason);
    }
}
