using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;

namespace KnightShift.Application.Mappers;

public static class GameStateMapper
{
    public static GameStateDto ToDto(GameState state)
    {
        return new GameStateDto
        {
            Board = MapBoard(state),
            CurrentTurn = state.CurrentTurn.ToString(),
            Result = state.Result.ToString(),
            EndReason = state.EndReason.ToString(),
            
            LastMove = state.MoveHistory.LastOrDefault() is Move lastMove 
                ? MoveMapper.ToDto(lastMove) 
                : null,
        };
    }

    private static PieceDto?[,] MapBoard(GameState state)
    {
        var board = new PieceDto?[BoardDimensions.Size, BoardDimensions.Size];

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var position = Position.CreateFromCoords(row, column);
                var piece = state.Board.GetPiece(position);

                board[row, column] = piece is null ? null : new PieceDto
                {
                    Type = piece.Type,
                    Color = piece.Color
                };
            }
        }

        return board;
    }
}
