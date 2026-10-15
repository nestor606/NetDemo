using NetDemo.Domain.Entities;

namespace NetDemo.Domain.Repositories;

public interface ITipoMercadoRepository
{
    Task<IReadOnlyList<TipoMercado>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<TipoMercado?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task GuardarAsync(TipoMercado tipoMercado, CancellationToken cancellationToken = default);

    Task<bool> EliminarAsync(Guid id, CancellationToken cancellationToken = default);
}
