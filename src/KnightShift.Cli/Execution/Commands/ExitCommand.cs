namespace KnightShift.Cli.Execution.Commands;

public class ExitCommand : ICommand
{
    public string Name => "exit";

    public bool CanHandle(string input)
        => input.Equals("exit", StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}
