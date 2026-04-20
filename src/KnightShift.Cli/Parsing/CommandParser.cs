using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Parsing;

public class CommandParser
{
    private readonly IEnumerable<ICommand> _commands;

    public CommandParser(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public ICommand? Parse(string input)
    {
        return _commands.FirstOrDefault(command => command.CanHandle(input));
    }
}
