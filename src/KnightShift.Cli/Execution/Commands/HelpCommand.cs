namespace KnightShift.Cli.Execution.Commands;

public class HelpCommand : ICommand
{
    public string Name => "help";

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        Console.WriteLine("""
            Available commands:
                move {uci}  → play specified move (e.g. move e2e4)
                list        → list legal moves for the current turn
                board       → show current board state
                exit        → exit application
                help        → show this help
            """
        );

        return Task.CompletedTask;
    }
}
