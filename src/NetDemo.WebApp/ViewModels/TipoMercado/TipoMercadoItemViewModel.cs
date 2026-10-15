using NetDemo.Application.TipoMercado;

namespace NetDemo.WebApp.ViewModels.TipoMercado;

public sealed record TipoMercadoItemViewModel(Guid Id, string Nombre, EstadoTipoMercadoDto Estado);
