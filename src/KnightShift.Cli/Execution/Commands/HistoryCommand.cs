using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class HistoryCommand : ICommand
{
    private readonly IGameService _game;
    private readonly IMoveFormatter _formatter;

    public CommandInfo Info => new(
        Name: "history",
        Aliases: ["san"],
        Parameter: null,
        Description: "Show move history (SAN)",
        Category: "View",
        Order: 3
    );

    public HistoryCommand(IGameService game, IMoveFormatter formatter)
    {
        _game = game;
        _formatter = formatter;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task ExecuteAsync(string input)
    {
        var history = _game.GetHistory().ToList();

        if (history.Count == 0)
        {
            Console.WriteLine("No moves played.");
            return Task.CompletedTask;
        }

        for (int i = 0; i < history.Count; i += 2)
        {
            int moveNumber = i / 2 + 1;

            var whiteMoveStep = history[i];
            var blackMoveStep = i + 1 < history.Count ? history[i + 1] : null;

            var whiteSan = _formatter.Format(
                whiteMoveStep.Move,
                whiteMoveStep.StateBeforeMove,
                whiteMoveStep.StateAfterMove
            );

            var blackSan = blackMoveStep is not null
                ? _formatter.Format(
                    blackMoveStep.Move,
                    blackMoveStep.StateBeforeMove,
                    blackMoveStep.StateAfterMove
                )
                : "";

            Console.WriteLine($"{moveNumber,2}. {whiteSan,-8} {blackSan,-8}");
        }

        return Task.CompletedTask;
    }
}
