namespace NetDemo.Application.TipoMercado;

public interface ITipoMercadoService
{
    Task<IReadOnlyList<TipoMercadoDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<TipoMercadoDto> RegistrarAsync(string nombre, CancellationToken cancellationToken = default);

    Task<TipoMercadoDto?> EditarAsync(
        Guid id,
        string nombre,
        EstadoTipoMercadoDto estado,
        CancellationToken cancellationToken = default);

    Task<bool> EliminarAsync(Guid id, CancellationToken cancellationToken = default);
}
