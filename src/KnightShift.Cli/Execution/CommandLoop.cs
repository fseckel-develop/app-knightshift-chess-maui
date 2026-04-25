using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution;

public class CommandLoop
{
    private readonly CommandRegistry _registry;

    public CommandLoop(CommandRegistry registry)
    {
        _registry = registry;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var command = _registry.FindCommand(input);

            if (command is null)
            {
                Console.WriteLine("Unknown command.");
                continue;
            }

            await command.ExecuteAsync(input);
        }
    }
}
