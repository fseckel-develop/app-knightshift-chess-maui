using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Execution;

public class CommandRegistry
{
    private readonly IEnumerable<ICommand> _commands;

    public CommandRegistry(IEnumerable<ICommand> commands)
    {
        _commands = commands;

        var helpCommand = _commands.OfType<HelpCommand>().FirstOrDefault();
        helpCommand?.SetCommands(_commands);
    }

    public IEnumerable<ICommand> GetAllCommands()
        => _commands;

    public ICommand? FindCommand(string input)
    {
        return _commands.FirstOrDefault(command => command.CanHandle(input));
    }
}
