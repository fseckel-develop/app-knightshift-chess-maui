using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game;
using KnightShift.Domain.Core;

namespace KnightShift.Infrastructure.Notation;

public class PgnGameImporter : IGameImporter
{
    private readonly IGameStateSerializer _serializer;
    private readonly IGameStateFactory _factory;
    private readonly SanMoveResolver _resolver;

    public PgnGameImporter(
        IGameStateSerializer serializer, 
        IGameStateFactory factory, 
        SanMoveResolver resolver)
    {
        _serializer = serializer;
        _factory = factory;
        _resolver = resolver;
    }

    public GameRecord Import(string pgn)
    {
        var fen = ExtractFen(pgn);
        var initialState = fen is not null
            ? _serializer.Deserialize(fen)
            : _factory.CreateInitialState();

        var moves = new List<Move>();
        var currentState = initialState;
        var tokens = Tokenize(pgn);

        int index = 0;
        foreach (var rawToken in tokens)
        {
            var token = rawToken.TrimEnd('+', '#', '!', '?');

            if (IsResultToken(token))
                continue;

            try
            {
                var move = _resolver.Resolve(token, currentState);
                moves.Add(move);
                currentState = currentState.ApplyMove(move);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error parsing move '{token}' at index {index}: {ex.Message}", ex);
            }

            index++;
        }

        return new GameRecord(initialState, moves);
    }

    private static string? ExtractFen(string pgn)
    {
        var lines = pgn.Split('\n');

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("[FEN "))
            {
                var fenStart = trimmedLine.IndexOf('"') + 1;
                var fenEnd = trimmedLine.LastIndexOf('"');

                return trimmedLine[fenStart..fenEnd];
            }
        }

        return null;
    }

    private static IEnumerable<string> Tokenize(string pgn)
    {
        var lines = pgn.Split('\n');

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("["))
                continue;

            var lineParts = trimmedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            bool inComment = false;
            bool inVariation = false;

            foreach (var linePart in lineParts)
            {
                inComment = linePart.StartsWith("{");
                inVariation = linePart.StartsWith("(");

                if (inComment || inVariation)
                {
                    inComment = !linePart.EndsWith("}");
                    inVariation = !linePart.EndsWith(")");
                    continue;
                }

                if (linePart.EndsWith("."))
                    continue;

                yield return linePart.Trim();
            }
        }
    }

    private static bool IsResultToken(string token)
    {
        return token is "1-0" or "0-1" or "1/2-1/2" or "*";
    }
}
