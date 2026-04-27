using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.Panels;

namespace KnightShift.Cli.Execution.Commands;

public class BoardCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "board",
        Aliases: ["display"],
        Parameter: null,
        Description: "Display current board",
        Category: "View",
        Order: 1
    );

    public BoardCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task ExecuteAsync(string input)
    {
        var state = _game.GetState();
        var board = BoardPanelRenderer.Render(state);
        Console.Write(board);
        
        return Task.CompletedTask;
    }
}
