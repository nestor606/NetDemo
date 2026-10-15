using Microsoft.AspNetCore.Mvc;
using NetDemo.Application.TipoMercado;
using NetDemo.WebApp.ViewModels.TipoMercado;

namespace NetDemo.WebApp.Controllers;

[Route("[controller]")]
public sealed class TipoMercadoController(ITipoMercadoService tipoMercadoService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tipos = await tipoMercadoService.ObtenerTodosAsync(cancellationToken);
        var viewModel = new TipoMercadoIndexViewModel(
            tipos.Select(tipo => new TipoMercadoItemViewModel(tipo.Id, tipo.Nombre, tipo.Estado)).ToArray());

        return View(viewModel);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(
        [FromForm] RegistrarTipoMercadoViewModel viewModel,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var tipo = await tipoMercadoService.RegistrarAsync(viewModel.Nombre, cancellationToken);
        return Ok(tipo);
    }

    [HttpPut("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        Guid id,
        [FromForm] EditarTipoMercadoViewModel viewModel,
        CancellationToken cancellationToken)
    {
        if (viewModel.Id != id)
            ModelState.AddModelError(nameof(viewModel.Id), "El identificador no coincide.");

        if (viewModel.Estado.HasValue && !Enum.IsDefined(viewModel.Estado.Value))
            ModelState.AddModelError(nameof(viewModel.Estado), "El estado no es válido.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var tipo = await tipoMercadoService.EditarAsync(
            id,
            viewModel.Nombre,
            viewModel.Estado!.Value,
            cancellationToken);

        return tipo is null ? NotFound() : Ok(tipo);
    }

    [HttpDelete("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var eliminado = await tipoMercadoService.EliminarAsync(id, cancellationToken);
        return eliminado ? NoContent() : NotFound();
    }
}
