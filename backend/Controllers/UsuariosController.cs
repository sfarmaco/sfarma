using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;
    public UsuariosController(IUsuarioService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<UsuarioDto>>> Get() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id) => (await _service.GetByIdAsync(id)) is { } u ? Ok(u) : NotFound();

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Post([FromBody] UsuarioCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
