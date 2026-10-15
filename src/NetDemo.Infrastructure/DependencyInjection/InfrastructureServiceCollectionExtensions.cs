using Microsoft.Extensions.DependencyInjection;
using NetDemo.Domain.Repositories;
using NetDemo.Infrastructure.Persistence;

namespace NetDemo.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string tipoMercadoDirectory)
    {
        services.AddSingleton<ITipoMercadoRepository>(
            new JsonTipoMercadoRepository(tipoMercadoDirectory));

        return services;
    }
}
