using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public interface IContentResolver
{
    string[] Resolve(UiState state);
}
