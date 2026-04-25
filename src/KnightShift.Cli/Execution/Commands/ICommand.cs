namespace KnightShift.Cli.Execution.Commands;

public interface ICommand
{
    CommandInfo Info { get; }
    bool CanHandle(string input);
    Task ExecuteAsync(string input);
}
