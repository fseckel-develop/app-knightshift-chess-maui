using KnightShift.Application.DTOs;
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
            EndReason = state.EndReason.ToString()
        };
    }

    private static string[][] MapBoard(GameState state)
    {
        var board = new string[BoardDimensions.Size][];

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            board[row] = new string[BoardDimensions.Size];

            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var position = Position.CreateFromCoords(row, column);
                var piece = state.Board.GetPiece(position);

                board[row][column] = piece is null
                    ? "."
                    : MapPiece(piece);
            }
        }

        return board;
    }

    private static string MapPiece(Piece piece)
    {
        var symbol = piece.Type switch
        {
            PieceType.Pawn => "p",
            PieceType.Knight => "n",
            PieceType.Bishop => "b",
            PieceType.Rook => "r",
            PieceType.Queen => "q",
            PieceType.King => "k",
            _ => "?"
        };

        return piece.Color == PieceColor.White
            ? symbol.ToUpper()
            : symbol;
    }
}
