using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public interface IContentProvider
{
    UiContent ContentType { get; }
    string[] GetContent(UiState state);
}
