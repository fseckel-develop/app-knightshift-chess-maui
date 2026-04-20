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
                exit    exit application
                help    show this help
            """
        );

        return Task.CompletedTask;
    }
}
