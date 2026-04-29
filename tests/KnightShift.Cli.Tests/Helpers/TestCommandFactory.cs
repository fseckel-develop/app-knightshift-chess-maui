using KnightShift.Cli.Execution;
using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Tests.Helpers;

public static class TestCommandFactory
{
    public static ICommand Create(
        string name,
        string category,
        int order,
        string description = "",
        string? parameter = null,
        params string[] aliases)
    {
        return new TestCommand(new CommandInfo(
            name,
            aliases,
            parameter,
            description,
            category,
            order
        ));
    }
}
