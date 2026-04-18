using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;

namespace KnightShift.Infrastructure.Serialization;

public class FenGameStateSerializer : IGameStateSerializer
{
    public GameState Deserialize(string fen)
    {
        var parts = fen.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 4)
            throw new ArgumentException("Invalid FEN: not enough parts.");

        var state = new GameState();

        ParseBoard(parts[0], state);
        state.CurrentTurn = ParseActiveColor(parts[1]);
        ParseCastlingRights(parts[2], state);
        state.EnPassantTarget = ParseEnPassant(parts[3]);

        return state;
    }

    public string Serialize(GameState state)
    {
        var board = BuildBoardString(state);
        var turn = state.CurrentTurn == PieceColor.White ? "w" : "b";
        var castling = BuildCastlingString(state);
        var enPassant = state.EnPassantTarget?.ToString() ?? "-";
        
        return $"{board} {turn} {castling} {enPassant}";
    }

    private static void ParseBoard(string boardPart, GameState state)
    {
        var ranks = boardPart.Split('/');

        if (ranks.Length != BoardDimensions.Size)
            throw new ArgumentException("Invalid FEN: board must have 8 ranks.");

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            int column = 0;

            foreach (char symbol in ranks[row])
            {
                if (char.IsDigit(symbol))
                {
                    column += symbol - '0';
                    continue;
                }

                var piece = ParsePiece(symbol);
                var position = Position.CreateFromCoords(row, column);
                state.Board.SetPiece(position, piece);

                column++;
            }

            if (column != BoardDimensions.Size)
                throw new ArgumentException("Invalid FEN: rank does not sum up to 8.");
        }
    }

    private static Piece ParsePiece(char symbol)
    {
        var type = char.ToLower(symbol) switch
        {
            'p' => PieceType.Pawn,
            'n' => PieceType.Knight,
            'b' => PieceType.Bishop,
            'r' => PieceType.Rook,
            'q' => PieceType.Queen,
            'k' => PieceType.King,
            _ => throw new ArgumentException($"Invalid FEN piece: {symbol}")
        };
        var color = char.IsUpper(symbol) ? PieceColor.White : PieceColor.Black;
        return new Piece(type, color);
    }

    private static PieceColor ParseActiveColor(string part)
    {
        return part switch
        {
            "w" => PieceColor.White,
            "b" => PieceColor.Black,
            _ => throw new ArgumentException($"Invalid FEN: {part} is not a valid active color.")
        };
    }

    private static void ParseCastlingRights(string part, GameState state)
    {
        state.WhiteCanCastleKingSide  = part.Contains('K');
        state.WhiteCanCastleQueenSide = part.Contains('Q');
        state.BlackCanCastleKingSide  = part.Contains('k');
        state.BlackCanCastleQueenSide = part.Contains('q');
    }

    private static Position? ParseEnPassant(string part)
    {
        return part == "-"
            ? null
            : Position.CreateFromAlgebraic(part);
    }

    private static string BuildBoardString(GameState state)
    {
        var ranks = new List<string>();
        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            ranks.Add(BuildRankString(state, row));
        }
        return string.Join("/", ranks);
    }

    private static string BuildRankString(GameState state, int row)
    {
        int emptyCount = 0;
        var builder = new System.Text.StringBuilder();

        for (int column = 0; column < BoardDimensions.Size; column++)
        {
            var position = Position.CreateFromCoords(row, column);
            var piece = state.Board.GetPiece(position);

            if (piece is null)
            {
                emptyCount++;
                continue;
            }

            if (emptyCount > 0)
            {
                builder.Append(emptyCount);
                emptyCount = 0;
            }

            builder.Append(GetFenChar(piece));
        }

        if (emptyCount > 0)
            builder.Append(emptyCount);

        return builder.ToString();
    }

    private static char GetFenChar(Piece piece)
    {
        char symbol = piece.Type switch
        {
            PieceType.Pawn   => 'p',
            PieceType.Knight => 'n',
            PieceType.Bishop => 'b',
            PieceType.Rook   => 'r',
            PieceType.Queen  => 'q',
            PieceType.King   => 'k',
            _ => throw new ArgumentException($"Invalid FEN: {piece.Type} is not a valid piece type.")
        };

        return piece.Color == PieceColor.White ? char.ToUpper(symbol) : symbol;
    }

    private static string BuildCastlingString(GameState state)
    {
        var castling = "";

        if (state.WhiteCanCastleKingSide)  castling += "K";
        if (state.WhiteCanCastleQueenSide) castling += "Q";
        if (state.BlackCanCastleKingSide)  castling += "k";
        if (state.BlackCanCastleQueenSide) castling += "q";

        return string.IsNullOrEmpty(castling) ? "-" : castling;
    }
}
