using Microsoft.Extensions.DependencyInjection;
using KnightShift.Cli.Execution;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Parsing;

namespace KnightShift.Cli.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCli(this IServiceCollection services)
    {
        services.AddScoped<App>();
        services.AddScoped<CommandLoop>();
        services.AddScoped<CommandParser>();

        services.AddScoped<ICommand, MoveCommand>();
        services.AddScoped<ICommand, ListMovesCommand>();
        services.AddScoped<ICommand, ShowBoardCommand>();
        services.AddScoped<ICommand, StatusCommand>();

        services.AddScoped<ICommand, NewGameCommand>();
        services.AddScoped<ICommand, UndoCommand>();
        services.AddScoped<ICommand, RedoCommand>();
        services.AddScoped<ICommand, ExitCommand>();

        services.AddScoped<ICommand, HelpCommand>();

        return services;
    }
}
