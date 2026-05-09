using KnightShift.Application.UseCases.GetState;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering;

namespace KnightShift.Cli.Execution;

public class CommandLoop
{
    private readonly CommandRegistry _registry;
    private readonly UiRenderer _renderer;
    private readonly UiStateUpdater _updater;
    private readonly UiState _uiState;

    public CommandLoop(
        CommandRegistry registry, 
        UiRenderer renderer, 
        UiStateUpdater updater,
        GetStateHandler getState)
    {
        _registry = registry;
        _renderer = renderer;
        _updater = updater;
        
        _uiState = new UiState
        { 
            Game = getState.Handle(new GetStateQuery()),
            ContentType = UiContent.Help,
            StatusMessage = "Let's play Chess with KnightShift CLI"
        };
    }

    public async Task RunAsync()
    {
        while (true)
        {
            if (_uiState.Mode == UiMode.Dashboard)
                Console.Clear();

            var ui = _renderer.Render(_uiState);
            Console.WriteLine(ui);

            if (_uiState.ExitRequested)
                break;
                
            Console.Write("> ");

            var input = Console.ReadLine() ?? "";
            var previousMode = _uiState.Mode;

            if (string.IsNullOrWhiteSpace(input))
                continue;
            
            CommandResult result;

            try
            {
                var command = _registry.FindCommand(input);

                if (command is null)
                {
                    _uiState.StatusMessage = "Unknown command.";
                    continue;
                }

                result = await command.ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                _uiState.StatusMessage = ex.Message;
                continue;
            }

            _updater.Apply(_uiState, result);

            if (previousMode != _uiState.Mode)
            {
                Console.Clear();
                Console.WriteLine("> " + input);
            }
        }
    }
}
