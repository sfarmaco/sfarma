using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;

namespace Sfarma.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Sales,Purchase,Accounting,Inventory")]
public class DashboardController : ControllerBase
{
    private readonly SfarmaContext _context;
    public DashboardController(SfarmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardSummaryDto>> Get()
    {
        var productos = await _context.Productos.CountAsync();
        var partners = await _context.Partners.CountAsync();
        var saleOrders = await _context.SaleOrders.CountAsync();
        var saleTotal = await _context.SaleOrders.SumAsync(o => (decimal?)o.Total) ?? 0m;
        var purchaseOrders = await _context.PurchaseOrders.CountAsync();
        var purchaseTotal = await _context.PurchaseOrders.SumAsync(o => (decimal?)o.Total) ?? 0m;
        var invoices = await _context.Invoices.CountAsync();
        var invoicesTotal = await _context.Invoices.SumAsync(o => (decimal?)o.Total) ?? 0m;

        var dto = new DashboardSummaryDto(productos, partners, saleOrders, saleTotal, purchaseOrders, purchaseTotal, invoices, invoicesTotal);
        return Ok(dto);
    }
}
