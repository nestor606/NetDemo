using NetDemo.Domain.Entities;
using NetDemo.Domain.Repositories;
using MarketType = NetDemo.Domain.Entities.TipoMercado;

namespace NetDemo.Application.TipoMercado;

public sealed class TipoMercadoService(ITipoMercadoRepository repository) : ITipoMercadoService
{
    public async Task<IReadOnlyList<TipoMercadoDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var tipos = await repository.ObtenerTodosAsync(cancellationToken);
        return tipos.Select(ToDto).ToArray();
    }

    public async Task<TipoMercadoDto> RegistrarAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var tipoMercado = MarketType.Registrar(Guid.NewGuid(), nombre);
        await repository.GuardarAsync(tipoMercado, cancellationToken);
        return ToDto(tipoMercado);
    }

    public async Task<TipoMercadoDto?> EditarAsync(
        Guid id,
        string nombre,
        EstadoTipoMercadoDto estado,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(estado))
            throw new ArgumentOutOfRangeException(nameof(estado));

        var tipoMercado = await repository.ObtenerPorIdAsync(id, cancellationToken);
        if (tipoMercado is null)
            return null;

        var estadoDominio = estado switch
        {
            EstadoTipoMercadoDto.Activo => EstadoTipoMercado.Activo,
            EstadoTipoMercadoDto.Inactivo => EstadoTipoMercado.Inactivo,
            _ => throw new ArgumentOutOfRangeException(nameof(estado))
        };

        tipoMercado.Editar(nombre, estadoDominio);
        await repository.GuardarAsync(tipoMercado, cancellationToken);
        return ToDto(tipoMercado);
    }

    public Task<bool> EliminarAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.EliminarAsync(id, cancellationToken);

    private static TipoMercadoDto ToDto(MarketType tipoMercado) => new(
        tipoMercado.Id,
        tipoMercado.Nombre,
        tipoMercado.Estado switch
        {
            EstadoTipoMercado.Activo => EstadoTipoMercadoDto.Activo,
            EstadoTipoMercado.Inactivo => EstadoTipoMercadoDto.Inactivo,
            _ => throw new ArgumentOutOfRangeException(nameof(tipoMercado.Estado))
        });
}
