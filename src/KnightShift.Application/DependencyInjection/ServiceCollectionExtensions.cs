using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.UseCases;
using KnightShift.Application.Game.Models;
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

        services.AddScoped<IQueryHandler<UseCases.ExportGame.ExportGameQuery, string>, UseCases.ExportGame.ExportGameHandler>();
        services.AddScoped<IQueryHandler<UseCases.ExportState.ExportStateQuery, string>, UseCases.ExportState.ExportStateHandler>();
        services.AddScoped<IQueryHandler<UseCases.GetHistory.GetHistoryQuery, IEnumerable<MoveStep>>, UseCases.GetHistory.GetHistoryHandler>();
        services.AddScoped<IQueryHandler<UseCases.GetMoves.GetMovesQuery, IEnumerable<MoveDto>>, UseCases.GetMoves.GetMovesHandler>();
        services.AddScoped<IQueryHandler<UseCases.GetState.GetStateQuery, GameStateDto>, UseCases.GetState.GetStateHandler>();
        services.AddScoped<ICommandHandler<UseCases.LoadGame.LoadGameCommand>, UseCases.LoadGame.LoadGameHandler>();
        services.AddScoped<ICommandHandler<UseCases.LoadState.LoadStateCommand>, UseCases.LoadState.LoadStateHandler>();
        services.AddScoped<ICommandHandler<UseCases.NewGame.NewGameCommand>, UseCases.NewGame.NewGameHandler>();
        services.AddScoped<ICommandHandler<UseCases.PlayMove.PlayMoveCommand>, UseCases.PlayMove.PlayMoveHandler>();
        services.AddScoped<IQueryHandler<UseCases.RedoMove.RedoMoveCommand, MoveDto?>, UseCases.RedoMove.RedoMoveHandler>();
        services.AddScoped<IQueryHandler<UseCases.UndoMove.UndoMoveCommand, MoveDto?>, UseCases.UndoMove.UndoMoveHandler>();

        return services;
    }
}
