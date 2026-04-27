using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering;

namespace KnightShift.Cli.Execution;

public class CommandLoop
{
    private readonly CommandRegistry _registry;
    private readonly IGameService _game;
    private readonly UiRenderer _renderer;
    private readonly UiState _uiState;

    public CommandLoop(CommandRegistry registry, UiRenderer renderer, IGameService game)
    {
        _registry = registry;
        _renderer = renderer;
        _game = game;
        
        _uiState = new UiState
        { 
            Game = game.GetState(),
            ContentType = UiContent.Help,
            StatusMessage = "Let's play Chess with KnightShift CLI"
        };
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            var ui = _renderer.Render(_uiState);
            Console.WriteLine(ui);
            Console.Write("> ");

            var input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var command = _registry.FindCommand(input);

            if (command is null)
            {
                _uiState.StatusMessage = "Unknown command.";
                continue;
            }

            CommandResult result;

            try
            {
                result = await command.ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                _uiState.StatusMessage = ex.Message;
                continue;
            }

            if (result.ExitRequested)
            {
                Console.WriteLine("  Exiting the application ... \n");
                break;
            }

            ApplyResult(result);
        }
    }

    private void ApplyResult(CommandResult result)
    {
        if (result.RefreshGameState)
        {
            _uiState.Game = _game.GetState();
            _uiState.ContentType = UiContent.History;
        }

        _uiState.ContentType = result.ContentType is not null 
            ? result.ContentType.Value 
            : UiContent.History;

        _uiState.StatusMessage = result.Message is not null
            ? result.Message
            : "";
    }
}
