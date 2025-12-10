using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
    private readonly IVentaService _service;
    public VentasController(IVentaService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<VentaDto>>> Get() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VentaDto>> GetById(int id) => (await _service.GetByIdAsync(id)) is { } v ? Ok(v) : NotFound();

    [HttpPost]
    public async Task<ActionResult<VentaDto>> Post([FromBody] VentaCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
