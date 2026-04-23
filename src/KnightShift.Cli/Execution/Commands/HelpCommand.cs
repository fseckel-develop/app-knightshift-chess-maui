namespace KnightShift.Cli.Execution.Commands;

public class HelpCommand : ICommand
{
    public string Name => "help";

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        Console.WriteLine("""
            Available commands:
                move {uci}    → play specified move (e.g. move e2e4)
                list          → list legal moves for the current turn
                list {square} → list legal moves for specified origin (e.g. list e2)
                board         → show current board state
                status        → show current game status
                history       → show move history in SAN notation
                fen           → show current position in FEN notation
                undo          → undo last move
                redo          → redo last undone move
                new           → start a new game
                load {fen}    → load game from string in FEN notation
                exit          → exit application
                help          → show this help
            """
        );

        return Task.CompletedTask;
    }
}
