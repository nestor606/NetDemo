namespace NetDemo.Application.TipoMercado;

public enum EstadoTipoMercadoDto
{
    Activo,
    Inactivo
}

public sealed record TipoMercadoDto(Guid Id, string Nombre, EstadoTipoMercadoDto Estado);
