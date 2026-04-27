namespace KnightShift.Cli.Execution.Commands;

public class BoardCommand : ICommand
{
    public CommandInfo Info => new(
        Name: "board",
        Aliases: ["display"],
        Parameter: null,
        Description: "Display current board",
        Category: "View",
        Order: 1
    );

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        return Task.FromResult(new CommandResult
        {
            RefreshGameState = true
        });
    }
}
