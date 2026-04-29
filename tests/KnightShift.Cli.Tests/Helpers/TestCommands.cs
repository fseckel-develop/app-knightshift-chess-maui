using KnightShift.Cli.Execution;
using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Tests.Helpers;

public class TestCommand : ICommand
{
    public CommandInfo Info { get; }

    public TestCommand(CommandInfo info)
    {
        Info = info;
    }

    public bool CanHandle(string input) => false;

    public Task<CommandResult> ExecuteAsync(string input)
        => Task.FromResult(new CommandResult());
}

public class FakeEchoCommand : ICommand
{
    private readonly string _trigger;

    public FakeEchoCommand(string trigger)
    {
        _trigger = trigger;
    }

    public CommandInfo Info => new(_trigger, [], null, "", "Test", 0);

    public bool CanHandle(string input) => input == _trigger;

    public Task<CommandResult> ExecuteAsync(string input)
    {
        return Task.FromResult(new CommandResult
        {
            Message = $"Executed {input}"
        });
    }
}

public class FakeExitCommand : ICommand
{
    public CommandInfo Info => new("exit", [], null, "", "System", 0);

    public bool CanHandle(string input) => input == "exit";

    public Task<CommandResult> ExecuteAsync(string input)
    {
        return Task.FromResult(new CommandResult
        {
            ExitRequested = true
        });
    }
}
