using System.ComponentModel.DataAnnotations;
using NetDemo.Application.TipoMercado;

namespace NetDemo.WebApp.ViewModels.TipoMercado;

public sealed class EditarTipoMercadoViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public EstadoTipoMercadoDto? Estado { get; set; }
}
