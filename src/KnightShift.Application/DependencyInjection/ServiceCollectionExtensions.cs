using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Interfaces;
using KnightShift.Application.Services;

namespace KnightShift.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();

        return services;
    }
}
