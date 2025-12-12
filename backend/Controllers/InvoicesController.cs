using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Accounting,Sales,Purchase")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _service;
    public InvoicesController(IInvoiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> Get() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InvoiceDto>> GetById(int id)
    {
        var inv = await _service.GetByIdAsync(id);
        return inv is null ? NotFound() : Ok(inv);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Post([FromBody] InvoiceCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:int}/state/{state}")]
    public async Task<IActionResult> UpdateState(int id, string state)
    {
        if (!Enum.TryParse<InvoiceState>(state, true, out var parsed))
            return BadRequest("Estado no válido");
        var ok = await _service.UpdateStateAsync(id, parsed);
        return ok ? NoContent() : NotFound();
    }
}
