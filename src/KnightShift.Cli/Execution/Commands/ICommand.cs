namespace KnightShift.Cli.Execution.Commands;

public interface ICommand
{
    CommandInfo Info { get; }
    bool CanHandle(string input);
    Task<CommandResult> ExecuteAsync(string input);
}
