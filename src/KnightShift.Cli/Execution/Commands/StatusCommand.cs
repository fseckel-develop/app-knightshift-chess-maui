using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Cli.Execution.Commands;

public class StatusCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "status",
        Aliases: ["info"],
        Parameter: null,
        Description: "Show game status",
        Category: "View",
        Order: 2
    );

    public StatusCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var state = _game.GetState();

        if (state.GameResult != GameResultDto.Ongoing)
        {
            return Task.FromResult(new CommandResult
            {
                Message = $"Game over: {state.GameEndReason}"
            });
        }

        var message = $"Turn: {state.CurrentTurn}";

        if (state.CurrentIsInCheck)
            message += " (Check!)";

        return Task.FromResult(new CommandResult
        {
            Message = message
        });
    }
}
