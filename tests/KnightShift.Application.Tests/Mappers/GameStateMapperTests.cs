using KnightShift.Application.Mappers;
using KnightShift.Application.Contracts.DTOs;
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

        Assert.Equal(8, dto.Board.GetLength(0));
        Assert.Equal(8, dto.Board.GetLength(1));
    }

    [Fact]
    public void ToDto_ShouldMapEmptySquaresAsNull()
    {
        var state = new GameState();

        var dto = GameStateMapper.ToDto(state);

        for (int row = 0; row < 8; row++)
        for (int column = 0; column < 8; column++)
            Assert.Null(dto.Board[row, column]);
    }

    [Fact]
    public void ToDto_ShouldMapWhitePiecesCorrectly()
    {
        var state = new GameState();

        var position = Position.CreateFromAlgebraic("e4");
        state.Board.SetPiece(position, new Piece(PieceType.Queen, PieceColor.White));

        var dto = GameStateMapper.ToDto(state);
        var (row, column) = Position.ToCoords(position);

        var piece = dto.Board[row, column];

        Assert.NotNull(piece);
        Assert.Equal(PieceTypeDto.Queen, piece.Type);
        Assert.Equal(PieceColorDto.White, piece.Color);
    }

    [Fact]
    public void ToDto_ShouldMapBlackPieceCorrectly()
    {
        var state = new GameState();

        var position = Position.CreateFromAlgebraic("d5");
        state.Board.SetPiece(position, new Piece(PieceType.Knight, PieceColor.Black));

        var dto = GameStateMapper.ToDto(state);
        var (row, col) = Position.ToCoords(position);

        var piece = dto.Board[row, col];

        Assert.NotNull(piece);
        Assert.Equal(PieceTypeDto.Knight, piece!.Type);
        Assert.Equal(PieceColorDto.Black, piece.Color);
    }

    [Fact]
    public void ToDto_ShouldMapAllPieceTypesCorrectly()
    {
        var state = new GameState();

        var testCases = new[]
        {
            PieceType.Pawn,
            PieceType.Knight,
            PieceType.Bishop,
            PieceType.Rook,
            PieceType.Queen,
            PieceType.King,
        };

        for (int i = 0; i < testCases.Length; i++)
        {
            var pos = Position.CreateFromCoords(0, i);
            state.Board.SetPiece(pos, new Piece(testCases[i], PieceColor.Black));
        }

        var dto = GameStateMapper.ToDto(state);

        for (int i = 0; i < testCases.Length; i++)
        {
            var piece = dto.Board[0, i];

            Assert.NotNull(piece);
            Assert.Equal((PieceTypeDto)testCases[i], piece!.Type);
            Assert.Equal(PieceColorDto.Black, piece.Color);
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

        Assert.Equal(PieceColorDto.Black, dto.CurrentTurn);
        Assert.Equal(GameResultDto.Draw, dto.GameResult);
        Assert.Equal(GameEndReasonDto.Stalemate, dto.GameEndReason);
    }
}
