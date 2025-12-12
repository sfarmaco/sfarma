using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Sales")]
public class SaleOrdersController : ControllerBase
{
    private readonly ISaleOrderService _service;
    private readonly IInvoiceService _invoiceService;

    public SaleOrdersController(ISaleOrderService service, IInvoiceService invoiceService)
    {
        _service = service;
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleOrderDto>>> Get() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SaleOrderDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SaleOrderDto>> Post([FromBody] SaleOrderCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:int}/state/{state}")]
    public async Task<IActionResult> UpdateState(int id, string state)
    {
        if (!Enum.TryParse<SaleOrderState>(state, true, out var parsed))
            return BadRequest("Estado no válido");
        var ok = await _service.UpdateStateAsync(id, parsed);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/invoice")]
    public async Task<ActionResult<InvoiceDto>> CreateInvoiceFromSale(int id)
    {
        var inv = await _invoiceService.CreateFromSaleOrderAsync(id);
        return inv is null ? NotFound() : Ok(inv);
    }
}
