using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Parsing;

namespace KnightShift.Cli.Execution;

public class CommandLoop
{
    private readonly CommandParser _parser;
    private readonly IGameService _game;

    public CommandLoop(CommandParser parser, IGameService game)
    {
        _parser = parser;
        _game = game;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var command = _parser.Parse(input);

            if (command is null)
            {
                Console.WriteLine("Unknown command.");
                continue;
            }

            await command.ExecuteAsync(input);
        }
    }
}
