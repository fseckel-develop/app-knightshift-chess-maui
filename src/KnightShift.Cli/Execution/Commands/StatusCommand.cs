using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Cli.Execution.Commands;

public class StatusCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "status";

    public StatusCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        var state = _game.GetState();

        if (state.GameResult != GameResultDto.Ongoing)
        {
            Console.WriteLine($"Game over: {state.GameEndReason}");
        }
        else
        {
            Console.Write($"Turn: {state.CurrentTurn}");

            if (state.CurrentIsInCheck)
                Console.Write(" (Check!)");
            
            Console.WriteLine();
        }

        return Task.CompletedTask;
    }
}
