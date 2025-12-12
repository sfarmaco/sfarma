using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Purchase")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _service;
    public ProveedoresController(IProveedorService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<ProveedorDto>>> Get() => Ok(await _service.GetAllAsync());
    [HttpGet("{id:int}")] public async Task<ActionResult<ProveedorDto>> GetById(int id) => (await _service.GetByIdAsync(id)) is { } p ? Ok(p) : NotFound();
    [HttpPost] public async Task<ActionResult<ProveedorDto>> Post([FromBody] ProveedorCreateDto dto) { var created = await _service.CreateAsync(dto); return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, [FromBody] ProveedorCreateDto dto) => await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
