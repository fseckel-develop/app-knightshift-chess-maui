using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class NewCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "new",
        Aliases: ["n", "reset", "start"],
        Parameter: null,
        Description: "Start new game",
        Category: "Game",
        Order: 3
    );

    public NewCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        _game.StartNewGame();

        return Task.FromResult(new CommandResult
        {
            Message = "New game started.",
            RefreshGameState = true
        });
    }
}
