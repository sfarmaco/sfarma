using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _service;
    public InventoryController(IInventoryService service)
    {
        _service = service;
    }

    [HttpGet("locations")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations() => Ok(await _service.GetLocationsAsync());

    [HttpPost("locations")]
    public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] LocationCreateDto dto)
    {
        var created = await _service.CreateLocationAsync(dto);
        return CreatedAtAction(nameof(GetLocations), new { id = created.Id }, created);
    }

    [HttpGet("pickings")]
    public async Task<ActionResult<IEnumerable<PickingDto>>> GetPickings() => Ok(await _service.GetPickingsAsync());

    [HttpGet("pickings/{id:int}")]
    public async Task<ActionResult<PickingDto>> GetPicking(int id)
    {
        var p = await _service.GetPickingByIdAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost("pickings")]
    public async Task<ActionResult<PickingDto>> CreatePicking([FromBody] PickingCreateDto dto)
    {
        var created = await _service.CreatePickingAsync(dto);
        return CreatedAtAction(nameof(GetPicking), new { id = created.Id }, created);
    }

    [HttpPost("pickings/{id:int}/state/{state}")]
    public async Task<IActionResult> UpdatePickingState(int id, string state)
    {
        if (!Enum.TryParse<PickingState>(state, true, out var parsed))
            return BadRequest("Estado no válido");
        var ok = await _service.UpdatePickingStateAsync(id, parsed);
        return ok ? NoContent() : NotFound();
    }
}
