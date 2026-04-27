namespace KnightShift.Cli.Execution.Commands;

public class ExitCommand : ICommand
{
    public CommandInfo Info => new(
        Name: "exit",
        Aliases: ["quit", "x", "q"],
        Parameter: null,
        Description: "Exit the application",
        Category: "System",
        Order: 0
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
            ExitRequested = true
        });
    }
}
