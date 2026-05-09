using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.GetState;

namespace KnightShift.Cli.Execution.Commands;

public class StatusCommand : ICommand
{
    private readonly IQueryHandler<GetStateQuery, GameStateDto> _handler;

    public CommandInfo Info => new(
        Name: "status",
        Aliases: ["info"],
        Parameter: null,
        Description: "Show game status",
        Category: "View",
        Order: 2
    );

    public StatusCommand(IQueryHandler<GetStateQuery, GameStateDto> handler)
    {
        _handler = handler;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var state = _handler.Handle(new GetStateQuery());

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
