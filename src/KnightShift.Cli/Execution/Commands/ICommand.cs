namespace KnightShift.Cli.Execution.Commands;

public interface ICommand
{
    string Name { get; }
    bool CanHandle(string input);
    Task ExecuteAsync(string input);
}
