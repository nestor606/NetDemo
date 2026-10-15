namespace NetDemo.Domain.Entities;

public sealed class TipoMercado
{
    private TipoMercado(Guid id, string nombre, EstadoTipoMercado estado)
    {
        Id = id;
        Nombre = nombre;
        Estado = estado;
    }

    public Guid Id { get; private set; }

    public string Nombre { get; private set; }

    public EstadoTipoMercado Estado { get; private set; }

    public static TipoMercado Registrar(Guid id, string nombre)
    {
        ValidarId(id);
        ValidarNombre(nombre);

        return new TipoMercado(id, nombre, EstadoTipoMercado.Activo);
    }

    public static TipoMercado Restaurar(Guid id, string nombre, EstadoTipoMercado estado)
    {
        ValidarId(id);
        ValidarNombre(nombre);
        ValidarEstado(estado);

        return new TipoMercado(id, nombre, estado);
    }

    public void Editar(string nombre, EstadoTipoMercado estado)
    {
        ValidarNombre(nombre);
        ValidarEstado(estado);

        Nombre = nombre;
        Estado = estado;
    }

    private static void ValidarId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("El identificador es obligatorio.", nameof(id));
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
    }

    private static void ValidarEstado(EstadoTipoMercado estado)
    {
        if (!Enum.IsDefined(estado))
            throw new ArgumentOutOfRangeException(nameof(estado), "El estado no es válido.");
    }
}
