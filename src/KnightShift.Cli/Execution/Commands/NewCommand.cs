using KnightShift.Application.UseCases.NewGame;

namespace KnightShift.Cli.Execution.Commands;

public class NewCommand : ICommand
{
    private readonly NewGameHandler _handler;

    public CommandInfo Info => new(
        Name: "new",
        Aliases: ["n", "reset", "start"],
        Parameter: null,
        Description: "Start new game",
        Category: "Game",
        Order: 3
    );

    public NewCommand(NewGameHandler hander)
    {
        _handler = hander;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        _handler.Handle(new NewGameCommand());

        return Task.FromResult(new CommandResult
        {
            Message = "New game started.",
            RefreshGameState = true
        });
    }
}
