using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public class ContentResolver
{
    private readonly Dictionary<UiContent, IContentProvider> _providers;

    public ContentResolver(IEnumerable<IContentProvider> providers)
    {
        _providers = providers.ToDictionary(provider => provider.ContentType);
    }

    public string[] Resolve(UiState state)
    {
        if (!_providers.TryGetValue(state.ContentType, out var provider))
            return [""];

        return provider.GetContent(state);
    }
}
