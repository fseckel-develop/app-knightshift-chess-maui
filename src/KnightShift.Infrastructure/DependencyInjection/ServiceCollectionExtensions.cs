using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Infrastructure.Factories;
using KnightShift.Infrastructure.Formatting;
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

        return services;
    }
}
