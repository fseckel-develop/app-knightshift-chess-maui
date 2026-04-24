using System.Text;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Notation;

public class PgnGameExporter : IGameExporter
{
    private readonly IMoveFormatter _formatter;

    public PgnGameExporter(IMoveFormatter formatter)
    {
        _formatter = formatter;
    }

    public string Export(GameRecord record)
    {
        var stringBuilder = new StringBuilder();

        var finalState = Replay(record.InitialState, record.Moves);

        stringBuilder.AppendLine("[Event \"Casual Game\"]");
        stringBuilder.AppendLine("[Site \"KnightShift CLI\"]");
        stringBuilder.AppendLine($"[Date \"{DateTime.UtcNow:yyyy.MM.dd}\"]");
        stringBuilder.AppendLine($"[Result \"{GetResult(finalState)}\"]");
        stringBuilder.AppendLine();

        var currentState = record.InitialState.Clone();

        foreach (var (move, i) in record.Moves.Select((move, idx) => (move, idx)))
        {
            if (i % 2 == 0)
                stringBuilder.Append($"{i / 2 + 1}. ");

            var stateBeforeMove = currentState;
            var stateAfterMove = currentState.ApplyMove(move);
            var sanFormattedMove = _formatter.Format(move, stateBeforeMove, stateAfterMove);

            stringBuilder.Append(sanFormattedMove);

            if (i < record.Moves.Count() - 1)
                stringBuilder.Append(' ');

            currentState = stateAfterMove;
        }

        return stringBuilder.ToString().Trim();
    }

    private static GameState Replay(GameState initialState, IEnumerable<Move> moves)
    {
        var state = initialState.Clone();

        foreach (var move in moves)
            state = state.ApplyMove(move);

        return state;
    }

    private static string GetResult(GameState state) => state.Result switch
    {
        GameResult.WhiteWins => "1-0",
        GameResult.BlackWins => "0-1",
        GameResult.Draw => "1/2-1/2",
        _ => "*"
    };
}
