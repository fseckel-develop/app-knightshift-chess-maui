using Microsoft.Extensions.DependencyInjection;
using KnightShift.Cli.Execution;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering;

namespace KnightShift.Cli.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCli(this IServiceCollection services)
    {
        services.AddScoped<App>();
        services.AddScoped<IContentProvider, HelpContentProvider>();
        services.AddScoped<IContentProvider, HistoryContentProvider>();
        services.AddScoped<IContentProvider, MovesContentProvider>();
        services.AddScoped<ContentResolver>();
        services.AddScoped<UiRenderer>();
        services.AddScoped<CommandLoop>();
        services.AddScoped<CommandRegistry>();

        services.AddScoped<ICommand, MoveCommand>();
        services.AddScoped<ICommand, UndoCommand>();
        services.AddScoped<ICommand, RedoCommand>();
        services.AddScoped<ICommand, NewCommand>();

        services.AddScoped<ICommand, ListCommand>();
        services.AddScoped<ICommand, BoardCommand>();
        services.AddScoped<ICommand, StatusCommand>();
        services.AddScoped<ICommand, HistoryCommand>();

        services.AddScoped<ICommand, LoadCommand>();
        services.AddScoped<ICommand, FenCommand>();
        services.AddScoped<ICommand, PgnCommand>();

        services.AddScoped<ICommand, ExitCommand>();
        services.AddScoped<ICommand, HelpCommand>();

        return services;
    }
}
