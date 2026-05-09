using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game.Services;

namespace KnightShift.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();

        services.AddScoped<GameCommandService>();
        services.AddScoped<GameQueryService>();
        services.AddScoped<GameStorageService>();

        services.AddScoped<UseCases.ExportGame.ExportGameHandler>();
        services.AddScoped<UseCases.ExportState.ExportStateHandler>();
        services.AddScoped<UseCases.GetHistory.GetHistoryHandler>();
        services.AddScoped<UseCases.GetMoves.GetMovesHandler>();
        services.AddScoped<UseCases.GetState.GetStateHandler>();
        services.AddScoped<UseCases.LoadGame.LoadGameHandler>();
        services.AddScoped<UseCases.LoadState.LoadStateHandler>();
        services.AddScoped<UseCases.NewGame.NewGameHandler>();
        services.AddScoped<UseCases.PlayMove.PlayMoveHandler>();
        services.AddScoped<UseCases.RedoMove.RedoMoveHandler>();
        services.AddScoped<UseCases.UndoMove.UndoMoveHandler>();

        return services;
    }
}
