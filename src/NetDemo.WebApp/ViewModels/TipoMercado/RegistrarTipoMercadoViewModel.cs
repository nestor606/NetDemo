using System.ComponentModel.DataAnnotations;

namespace NetDemo.WebApp.ViewModels.TipoMercado;

public sealed class RegistrarTipoMercadoViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
}
