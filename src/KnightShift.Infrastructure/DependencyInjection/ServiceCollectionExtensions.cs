using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Infrastructure.Factories;
using KnightShift.Infrastructure.Notation;
using KnightShift.Infrastructure.Serialization;

namespace KnightShift.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IGameStateFactory, FenGameStateFactory>();
        services.AddScoped<IGameStateSerializer, FenGameStateSerializer>();
        services.AddScoped<IMoveSerializer, UciMoveSerializer>();
        services.AddScoped<IMoveFormatter, SanMoveFormatter>();
        services.AddScoped<SanMoveResolver>();
        services.AddScoped<IGameImporter, PgnGameImporter>();
        services.AddScoped<IGameExporter, PgnGameExporter>();

        return services;
    }
}
