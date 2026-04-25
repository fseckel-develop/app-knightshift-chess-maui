namespace KnightShift.Cli.Execution;

public sealed record CommandInfo
(
    string Name,
    IReadOnlyList<string> Aliases,
    string? Parameter,
    string Description,
    string Category,
    int Order
);
