using System.Text.Json;
using System.Text.Json.Serialization;
using NetDemo.Domain.Entities;
using NetDemo.Domain.Repositories;

namespace NetDemo.Infrastructure.Persistence;

public sealed class JsonTipoMercadoRepository : ITipoMercadoRepository
{
    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();
    private readonly string _directoryPath;

    public JsonTipoMercadoRepository(string directoryPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);
        _directoryPath = directoryPath;
        Directory.CreateDirectory(_directoryPath);
    }

    public async Task<IReadOnlyList<TipoMercado>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var tipos = new List<TipoMercado>();

        foreach (var filePath in Directory.EnumerateFiles(_directoryPath, "*.json"))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read | FileShare.Delete,
                bufferSize: 4096,
                useAsync: true);

            var storedType = await JsonSerializer.DeserializeAsync<TipoMercadoFile>(
                stream,
                JsonOptions,
                cancellationToken);

            if (storedType is null)
                throw new InvalidDataException($"El archivo '{Path.GetFileName(filePath)}' no contiene un tipo de mercado válido.");

            tipos.Add(storedType.ToDomain());
        }

        return tipos;
    }

    public async Task<TipoMercado?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var filePath = GetFilePath(id);
        if (!File.Exists(filePath))
            return null;

        await using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read | FileShare.Delete,
            bufferSize: 4096,
            useAsync: true);

        var storedType = await JsonSerializer.DeserializeAsync<TipoMercadoFile>(
            stream,
            JsonOptions,
            cancellationToken);

        return storedType?.ToDomain()
            ?? throw new InvalidDataException($"El archivo '{Path.GetFileName(filePath)}' no contiene un tipo de mercado válido.");
    }

    public async Task GuardarAsync(
        TipoMercado tipoMercado,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tipoMercado);
        Directory.CreateDirectory(_directoryPath);

        var filePath = GetFilePath(tipoMercado.Id);
        var temporaryPath = $"{filePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await JsonSerializer.SerializeAsync(
                    stream,
                    TipoMercadoFile.FromDomain(tipoMercado),
                    JsonOptions,
                    cancellationToken);
            }

            File.Move(temporaryPath, filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    public Task<bool> EliminarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filePath = GetFilePath(id);
        if (!File.Exists(filePath))
            return Task.FromResult(false);

        File.Delete(filePath);
        return Task.FromResult(true);
    }

    private string GetFilePath(Guid id) =>
        Path.Combine(_directoryPath, $"{id:N}.json");

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }

    private sealed class TipoMercadoFile
    {
        public TipoMercadoFile()
        {
        }

        public Guid Id { get; init; }

        public string Nombre { get; init; } = string.Empty;

        public EstadoTipoMercado Estado { get; init; }

        public TipoMercado ToDomain() =>
            TipoMercado.Restaurar(Id, Nombre, Estado);

        public static TipoMercadoFile FromDomain(TipoMercado tipoMercado) => new()
        {
            Id = tipoMercado.Id,
            Nombre = tipoMercado.Nombre,
            Estado = tipoMercado.Estado
        };
    }
}
