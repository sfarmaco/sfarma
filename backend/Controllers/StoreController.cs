using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly IVentaService _ventaService;
    public StoreController(IProductoService productoService, IVentaService ventaService)
    {
        _productoService = productoService;
        _ventaService = ventaService;
    }

    [HttpGet("productos")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos() =>
        Ok(await _productoService.GetAllAsync());

    [HttpPost("checkout")]
    [AllowAnonymous]
    public async Task<ActionResult<VentaDto>> Checkout([FromBody] VentaCreateDto dto)
    {
        if (dto == null || dto.Detalles is null || dto.Detalles.Count == 0)
            return BadRequest("Detalle de venta vacío");

        var sanitized = dto with
        {
            EmpleadoId = dto.EmpleadoId == 0 ? 1 : dto.EmpleadoId,
            Fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha,
            TipoVenta = string.IsNullOrWhiteSpace(dto.TipoVenta) ? "Online" : dto.TipoVenta
        };

        var venta = await _ventaService.CreateAsync(sanitized);
        return Ok(venta);
    }
}
